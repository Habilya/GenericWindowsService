using GenericWindowsService.Application.Process;
using GenericWindowsService.Application.Process.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWindowsService.Application.Factory;

public class GenericProcessFactory
{
	public GenericProcess MakeProcess(string processObjectCode, IServiceProvider serviceProvider)
	{
		return processObjectCode switch
		{
			GenericProcessFactoryConstants.PROCESS_CODE_OLD_FILES_REMOVER => serviceProvider.GetRequiredService<OldFilesRemover>(),

			GenericProcessFactoryConstants.PROCESS_SCAN_FOR_FILES_CODE => serviceProvider.GetRequiredService<ScanForParticularFiles>(),

			_ => throw new Exception($"Factory could not create a process object, Unknown ProcessObjectCode {processObjectCode}"),
		};
	}
}
