namespace GenericWindowsService.Application.ServiceConfiguration;

public class ServiceConfiguration
{
	public string ServiceName { get; init; } = default!;
	public int RunEveryMS { get; init; } = 3000;
	public int MaxDegreeOfParallelism { get; init; } = 5;
	public bool IsNetworkPingEnabled { get; init; } = false;
	public string NetworkPathToPing { get; init; } = "";

	public List<ScheduledProcessConfiguration> ScheduledProcesses { get; init; } = default!;
}
