using GenericWindowsService.Library.Logging;
using Microsoft.Extensions.Hosting;

namespace GenericWindowsService.Application;

public class GenericService : BackgroundService
{
	private readonly ILoggerAdapter<GenericService> _logger;

	public GenericService(ILoggerAdapter<GenericService> logger)
	{
		_logger = logger;
	}

	public override Task StartAsync(CancellationToken cancellationToken)
	{
		try
		{
			_logger.LogInformation("Service is starting...");
			_logger.LogInformation("Initializing configuration...");

			return base.StartAsync(cancellationToken);
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("GenericService StartAsync Task canceled.");
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
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				//WindowsServiceConfigure.RunnableServices.ForEach(s => s.ServiceThread());
				await Task.Delay(5000, stoppingToken);
			}
		}
		catch (TaskCanceledException)
		{
			_logger.LogWarning("GenericService ExecuteAsync Task canceled.");
		}
	}
}
