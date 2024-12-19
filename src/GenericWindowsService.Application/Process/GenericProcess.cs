using GenericWindowsService.Application.ServiceConfiguration;

namespace GenericWindowsService.Application.Process;

public class GenericProcess : IGenericProcess
{
	private readonly ScheduledProcessConfiguration _scheduledProcessConfiguration;

	public bool IsValid { get; set; } = default!;
	public List<string> ValidationMessages { get; set; } = default!;
	public DateTime NextRunTime { get; set; } = DateTime.MinValue;

	public ScheduledProcessConfiguration ProcessConfiguration
	{
		get { return _scheduledProcessConfiguration; }
	}

	public GenericProcess(ScheduledProcessConfiguration scheduledProcessConfiguration)
	{
		_scheduledProcessConfiguration = scheduledProcessConfiguration;
	}

	public void Validate()
	{
		// In the inhertided classes vrite some validations to populate ValidationMessages

		if (ValidationMessages.Any())
		{
			IsValid = false;
		}
	}

	public void RunProcess()
	{
		throw new NotImplementedException();
	}
}
