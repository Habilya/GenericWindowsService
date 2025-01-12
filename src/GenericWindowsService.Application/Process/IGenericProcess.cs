using GenericWindowsService.Application.ServiceConfiguration;

namespace GenericWindowsService.Application.Process;

public interface IGenericProcess
{
	bool IsValid { get; set; }
	void InitConfig(ScheduledProcessConfiguration config);
	void ValidateConfig();
	void RunProcess();
}
