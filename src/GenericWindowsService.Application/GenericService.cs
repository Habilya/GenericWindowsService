using GenericWindowsService.Application.Process;
using GenericWindowsService.Library.Extensions;
using GenericWindowsService.Library.Logging;
using GenericWindowsService.Library.Providers;
using System.Collections.Concurrent;

namespace GenericWindowsService.Application;

public class GenericService
{
	private readonly ILoggerAdapter<BackgroundWorker> _logger;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ICronSchedulingProvider _cronSchedulingProvider;
	private readonly ServiceConfiguration.ServiceConfiguration _serviceConfiguration;

	private readonly ConcurrentDictionary<string, Task> _runningProcesses = default!;

	protected List<GenericProcess> ProcessesToRun { get; set; } = default!;

	public GenericService(
		ILoggerAdapter<BackgroundWorker> logger,
		ServiceConfiguration.ServiceConfiguration serviceConfiguration,
		ICronSchedulingProvider cronSchedulingProvider,
		IDateTimeProvider dateTimeProvider)
	{
		_logger = logger;
		_dateTimeProvider = dateTimeProvider;
		_cronSchedulingProvider = cronSchedulingProvider;
		_serviceConfiguration = serviceConfiguration;

		_runningProcesses = new ConcurrentDictionary<string, Task>();

		InitializeProcessesToRun();
	}

	// TODO: Add Process Initialization (Factory?) 
	// populate ProcessesToRun
	public virtual void InitializeProcessesToRun()
	{
		throw new NotImplementedException();
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
		if (!ProcessPrevalidateBeforeRun(processToRun))
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

	private bool ProcessPrevalidateBeforeRun(GenericProcess processToRun)
	{
		throw new NotImplementedException();
	}

	private void UpdateNextRunTime(GenericProcess processToRun, DateTime currentTime)
	{
		processToRun.NextRunTime = _cronSchedulingProvider.GetNextRunFromSchedule(processToRun.ProcessConfiguration.Schedule);
	}

	private List<GenericProcess> PrepareProcessesToRun()
	{
		throw new NotImplementedException();
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
