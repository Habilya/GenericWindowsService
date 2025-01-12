using GenericWindowsService.Application.ServiceConfiguration;
using GenericWindowsService.Library.Logging;

namespace GenericWindowsService.Application.Process.Concrete;

public class OldFilesRemover : GenericProcess
{
	public OldFilesRemover(
		ILoggerAdapter<GenericProcess> loggerAdapter,
		ServiceGenericConfiguration serviceConfiguration)
		: base(loggerAdapter, serviceConfiguration)
	{
	}
}
