namespace GenericWindowsService.Application.ServiceConfiguration;

public class ScheduledProcessConfiguration
{
	public int RunEveryMS { get; init; } = 3000;
	public int MaxDegreeOfParallelism { get; init; } = 5;
	public bool IsNetworkPingEnabled { get; init; } = false;
	public string NetworkPathToPing { get; init; } = "";


	public string ProcessLabel { get; init; } = default!;
	public string ProcessCodeName { get; init; } = default!;
	public bool IsEnabled { get; init; } = false;
	public string Schedule { get; init; } = ConstantsProcess.DEFAULT_PROCESS_SCHEDULE;
	public Dictionary<string, string> AdHocFields { get; init; } = default!;
}
