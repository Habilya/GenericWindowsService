using GenericWindowsService.Application.Constants;

namespace GenericWindowsService.Application.ServiceConfiguration;

public class ScheduledProcessConfiguration
{
	public string ProcessLabel { get; init; } = default!;
	public string ProcessCodeName { get; init; } = default!;
	public bool IsEnabled { get; init; } = false;
	public bool IsNetworkDependant { get; init; } = false;
	public string Schedule { get; init; } = ScheduledProcess.DEFAULT_PROCESS_SCHEDULE;
	public Dictionary<string, object> AdHocFields { get; init; } = default!;

	public string LabelAndProcessCode { get { return $"{ProcessLabel}-{ProcessCodeName}"; } }
}
