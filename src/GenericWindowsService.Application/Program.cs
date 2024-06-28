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
			var ENV_DOTNET_ENVIRONMENT = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

			config.AddJsonFile($@"Config\appsettings.json", optional: false)
				.AddJsonFile($@"Config\appsettings.{ENV_DOTNET_ENVIRONMENT}.json", optional: true)
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
			var appSettings = new AppSettings();
			hostContext.Configuration.GetSection("AppSettings").Bind(appSettings);

			// Register the appSettings as a singleton service
			services.AddSingleton(appSettings);

			services.AddHostedService<GenericService>();
			services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
			services.AddSingleton<IGuidProvider, GuidProvider>();
			services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
		});
}
