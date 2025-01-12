using GenericWindowsService.Application.ServiceConfiguration;
using GenericWindowsService.Library.Logging;

namespace GenericWindowsService.Application.Process.Concrete;

public class ScanForParticularFiles : GenericProcess
{
	public ScanForParticularFiles(
		ILoggerAdapter<GenericProcess> loggerAdapter,
		ServiceGenericConfiguration serviceConfiguration)
		: base(loggerAdapter, serviceConfiguration)
	{
	}

	protected override void ConfigValidations()
	{

	}

	public override void RunProcess()
	{
		LogInformation("Process tick");
	}
}
