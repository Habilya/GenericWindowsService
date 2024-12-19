namespace GenericWindowsService.Application.Process;

public interface IGenericProcess
{
	bool IsValid { get; set; }

	void RunProcess();
}
