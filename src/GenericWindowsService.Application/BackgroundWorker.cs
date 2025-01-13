using GenericWindowsService.Library.Logging;
using Microsoft.Extensions.Hosting;

namespace GenericWindowsService.Application;

public class BackgroundWorker : BackgroundService
{
	private readonly ILoggerAdapter<BackgroundWorker> _logger;
	private readonly ServiceConfiguration.ServiceGenericConfiguration _serviceConfiguration;
	private readonly GenericService _genericService;

	public BackgroundWorker(
		ILoggerAdapter<BackgroundWorker> logger,
		ServiceConfiguration.ServiceGenericConfiguration serviceConfiguration,
		GenericService genericService)
	{
		_logger = logger;
		_serviceConfiguration = serviceConfiguration;
		_genericService = genericService;
	}

	public override Task StartAsync(CancellationToken cancellationToken)
	{
		try
		{
			_logger.LogInformation("Initializing configuration...");
			_logger.LogInformation("{appVersion} env: {envCodeName}",
				_serviceConfiguration.GetVersionOneLiner(),
				_serviceConfiguration.Environement);

			_serviceConfiguration.ValidateServiceLevelConfig();
			_genericService.InitializeProcessesToRun();

			_logger.LogInformation("Service {serviceName} is starting...", _serviceConfiguration.ServiceName);

			// base method StartAsync of BackgroundService calls the ExecuteAsync new the end...
			return base.StartAsync(cancellationToken);
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("{genericService} StartAsync Task canceled.", nameof(BackgroundWorker));
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled excpetion occured while starting service.");
			return Task.CompletedTask;
		}
	}

	public override Task StopAsync(CancellationToken cancellationToken)
	{
		try
		{
			_logger.LogInformation("Service is stopping...");
			_genericService.EndAllProcesses();
			return base.StopAsync(cancellationToken);
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("{genericService} StopAsync Task canceled.", nameof(BackgroundWorker));
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled excpetion occured while stopping service.");
			return Task.CompletedTask;
		}
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.Delay(_serviceConfiguration.RunEveryMS, stoppingToken);
				_genericService.ServiceThread();
			}
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("{genericService} ExecuteAsync Task canceled.", nameof(BackgroundWorker));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled excpetion occured while running service.");
			_logger.LogWarning("Service {genericService} is halted.", nameof(BackgroundWorker));
			throw;
		}
	}
}
