using GenericWindowsService.Application.Factory;
using GenericWindowsService.Application.Process;
using GenericWindowsService.Application.ServiceConfiguration;
using GenericWindowsService.Library.Extensions;
using GenericWindowsService.Library.Logging;
using GenericWindowsService.Library.Providers;
using System.Collections.Concurrent;

namespace GenericWindowsService.Application;

public class GenericService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILoggerAdapter<BackgroundWorker> _logger;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ICronSchedulingProvider _cronSchedulingProvider;
	private readonly ServiceGenericConfiguration _serviceConfiguration;
	private readonly GenericProcessFactory _genericProcessFactory;

	private readonly ConcurrentDictionary<string, Task> _runningProcesses = default!;

	protected List<GenericProcess> ProcessesToRun { get; set; } = default!;

	public GenericService(
		IServiceProvider serviceProvider,
		ILoggerAdapter<BackgroundWorker> logger,
		ServiceGenericConfiguration serviceConfiguration,
		ICronSchedulingProvider cronSchedulingProvider,
		IDateTimeProvider dateTimeProvider,
		GenericProcessFactory genericProcessFactory)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_dateTimeProvider = dateTimeProvider;
		_cronSchedulingProvider = cronSchedulingProvider;
		_serviceConfiguration = serviceConfiguration;
		_genericProcessFactory = genericProcessFactory;

		_runningProcesses = new ConcurrentDictionary<string, Task>();
	}

	public virtual void InitializeProcessesToRun()
	{
		ProcessesToRun = new List<GenericProcess>();

		var enabledProcessConfigs = _serviceConfiguration.ScheduledProcesses
			.Where(q => q.IsEnabled)
			.ToList();

		enabledProcessConfigs.ForEach(p =>
		{
			var createdProcess = _genericProcessFactory.MakeProcess(p.ProcessCodeName, _serviceProvider);
			createdProcess.InitConfig(p);
			createdProcess.ValidateConfig();

			if (createdProcess.IsValid)
			{
				UpdateNextRunTime(createdProcess, _dateTimeProvider.DateTimeNow);
				ProcessesToRun.Add(createdProcess);
			}
			else
			{
				_logger.LogWarning($"Invalid processes ({createdProcess.ScheduledProcessConfiguration.LabelAndProcessCode})");
			}
		});

		LogInitializedProcesses(enabledProcessConfigs);
		LogDisabledProcesses();
	}

	private void LogInitializedProcesses(List<ScheduledProcessConfiguration> enabledProcessConfigs)
	{
		_logger.LogInformation($"Initialized processes:");
		foreach (var p in ProcessesToRun)
		{
			_logger.LogInformation($"[{p.ScheduledProcessConfiguration.LabelAndProcessCode}] Schedule ({p.ScheduledProcessConfiguration.Schedule}), Next run at :{p.NextRunTime}");
		}
		_logger.LogInformation($"Initialized {ProcessesToRun.Count}/{enabledProcessConfigs.Count} processes.");
	}

	private void LogDisabledProcesses()
	{
		var disabledProcesses = _serviceConfiguration.ScheduledProcesses
			.Where(q => !q.IsEnabled)
			.ToList();

		if (disabledProcesses.Any())
		{
			_logger.LogWarning($"Disabled processes ({disabledProcesses.Count})");
			disabledProcesses.ForEach(p =>
			{
				_logger.LogWarning($"\t{p.LabelAndProcessCode}");
			});
		}
	}

	public virtual void ServiceThread()
	{
		// TODO: add network ping network stability check (ping network drive)
		// Validations before each run?
		var preparedProcesses = PrepareProcessesToRun();

		// TODO: Refactor needed
		foreach (var processToRun in preparedProcesses)
		{
			var currentTime = _dateTimeProvider.DateTimeNow;

			if (currentTime >= processToRun.NextRunTime)
			{
				var processSignature = processToRun.GetType().FullName;

				if (ShouldSkipThisProcess(processToRun, processSignature!))
				{
					continue;
				}

				var runningProcess = Task.Run(() =>
				{
					try
					{
						UpdateNextRunTime(processToRun, currentTime);
						processToRun.RunProcess();
					}
					catch (Exception)
					{
						throw;
					}
					finally
					{
						// remove the process for running processes
						_runningProcesses.TryRemove(processSignature!, out _);
					}
				});

				_runningProcesses.TryAdd(processSignature!, runningProcess);

				// Control the degree pf parallelism by awaiting processes if we reach the limit of active processes
				ControlTheConcurencyOfRunningProcesses();
			}
		}
	}

	private void ControlTheConcurencyOfRunningProcesses()
	{
		if (_runningProcesses.Count >= _serviceConfiguration.MaxDegreeOfParallelism)
		{
			Task.WaitAny(_runningProcesses.Select(q => q.Value).ToArray());

			var processesToRemove = _runningProcesses
				.Where(q => q.Value.IsCanceled || q.Value.IsCompleted)
				.Select(q => q.Key);

			processesToRemove.ForEach(q => _runningProcesses.TryRemove(q, out _));
		}
	}

	private bool ShouldSkipThisProcess(GenericProcess processToRun, string processSignature)
	{
		// Run the method that is intended with validations to be run before each run
		if (!processToRun.IsValidBeforeRun())
		{
			return true;
		}

		// Check if process is already running
		if (_runningProcesses.ContainsKey(processSignature))
		{
			// Skip this process since it's already running
			return true;
		}

		return false;
	}

	private void UpdateNextRunTime(GenericProcess processToRun, DateTime currentTime)
	{
		processToRun.NextRunTime = _cronSchedulingProvider.GetNextRunFromSchedule(processToRun.ScheduledProcessConfiguration.Schedule);
	}

	private List<GenericProcess> PrepareProcessesToRun()
	{
		return ProcessesToRun
			.Where(q => q.IsValid)
			.ToList();
	}

	public virtual void EndAllProcesses()
	{
		if (_runningProcesses is null || !_runningProcesses.Any())
		{
			return;
		}

		_runningProcesses.ForEach(q => q.Value.Wait(10));

		var runningProcesses = _runningProcesses.ToList();
		runningProcesses.ForEach(q => _runningProcesses.TryRemove(q));
	}
}
