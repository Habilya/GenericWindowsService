using GenericWindowsService.Application.ServiceConfiguration;
using GenericWindowsService.Library.Logging;
using GenericWindowsService.Library.Providers;

namespace GenericWindowsService.Application.Process.Concrete;

public class ScanForParticularFiles : GenericProcess
{
	private readonly IGuidProvider _guidProvider;

	public ScanForParticularFiles(IGuidProvider guidProvider,
		ILoggerAdapter<GenericProcess> loggerAdapter,
		ServiceGenericConfiguration serviceConfiguration)
		: base(loggerAdapter, serviceConfiguration)
	{
		_guidProvider = guidProvider;
	}

	protected override void ConfigValidations()
	{

	}

	public override void RunProcess()
	{
		LogInformation($"Process tick {_guidProvider.NewGuid()}");
	}
}
