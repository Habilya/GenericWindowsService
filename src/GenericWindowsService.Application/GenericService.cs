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

			return base.StartAsync(cancellationToken);
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("GenericService StartAsync Task canceled.");
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
			_logger.LogWarning("GenericService StopAsync Task canceled.");
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
				//WindowsServiceConfigure.RunnableServices.ForEach(s => s.ServiceThread());
				await Task.Delay(_serviceConfiguration.RunEveryMS, stoppingToken);
			}
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("GenericService ExecuteAsync Task canceled.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled excpetion occured while running service.");
		}
	}
}
