using GenericWindowsService.Library.Logging;
using GenericWindowsService.Library.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace GenericWindowsService.Application;

class Program
{
	public static void Main(string[] args)
	{
		Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

		CreateHostBuilder(args)
			.Build()
			.Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
		.ConfigureAppConfiguration((hostContext, config) =>
		{
			config.AddJsonFile(@"Config\appsettings.json", optional: false)
				.AddEnvironmentVariables()
				.Build();
		})
		.ConfigureLogging((hostContext, logging) =>
		{
			var serilogLogger = new LoggerConfiguration()
				.ReadFrom.Configuration(hostContext.Configuration)
				.Enrich.FromLogContext()
				.CreateLogger();

			logging.ClearProviders();
			logging.AddSerilog(serilogLogger);
		})
		.UseWindowsService()
		.ConfigureServices((hostContext, services) =>
		{
			// Read appsettings.json into a model
			var serviceConfiguration = new ServiceConfiguration.ServiceGenericConfiguration();
			hostContext.Configuration.GetSection(Constants.Config.APPSETTINGS_SERVICE_CONFIG_SECTION_NAME).Bind(serviceConfiguration);

			services.AddSingleton(serviceConfiguration);
			services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
			services.AddSingleton<ICronSchedulingProvider, CronSchedulingProvider>();
			services.AddSingleton<IGuidProvider, GuidProvider>();
			services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
			services.AddSingleton<GenericService>();

			// The Service itself
			services.AddHostedService<BackgroundWorker>();
		});
}
