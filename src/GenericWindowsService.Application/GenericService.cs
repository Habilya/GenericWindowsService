using GenericWindowsService.Library.Logging;
using Microsoft.Extensions.Hosting;

namespace GenericWindowsService.Application;

public class GenericService : BackgroundService
{
	private readonly ILoggerAdapter<GenericService> _logger;
	private readonly ServiceConfiguration.ServiceConfiguration _serviceConfiguration;

	public GenericService(
		ILoggerAdapter<GenericService> logger,
		ServiceConfiguration.ServiceConfiguration serviceConfiguration)
	{
		_logger = logger;
		_serviceConfiguration = serviceConfiguration;
	}

	public override Task StartAsync(CancellationToken cancellationToken)
	{
		try
		{
			_logger.LogInformation("Initializing configuration...");
			_logger.LogInformation("Version: {appVersion}-{buildNumber} env: {envCodeName}",
						  _serviceConfiguration.Version,
						  _serviceConfiguration.Build,
						  _serviceConfiguration.Environement);
			_logger.LogInformation("Service {serviceName} is starting...", _serviceConfiguration.ServiceName);

			// base method StartAsync of BackgroundService calls the ExecuteAsync new the end...
			return base.StartAsync(cancellationToken);
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("{genericService} StartAsync Task canceled.", nameof(GenericService));
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

			return base.StopAsync(cancellationToken);
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("{genericService} StopAsync Task canceled.", nameof(GenericService));
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
				//WindowsServiceConfigure.RunnableServices.ForEach(s => s.ServiceThread());
			}
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("{genericService} ExecuteAsync Task canceled.", nameof(GenericService));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled excpetion occured while running service.");
			_logger.LogWarning("Service {genericService} is halted.", nameof(GenericService));
		}
	}
}
