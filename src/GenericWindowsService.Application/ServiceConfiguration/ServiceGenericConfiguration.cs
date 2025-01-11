using GenericWindowsService.Application.Constants;
using System.Text.Json.Serialization;

namespace GenericWindowsService.Application.ServiceConfiguration;

public class ServiceGenericConfiguration
{
	public ServiceGenericConfiguration()
	{
		Version = Versionning.GetVersion();
		Build = Versionning.GetVersionPostfix();
	}

	public string ServiceName { get; init; } = default!;
	public string Environement { get; init; } = default!;
	public int RunEveryMS { get; init; } = 3000;
	public int MaxDegreeOfParallelism { get; init; } = 3;
	public bool IsNetworkPingEnabled { get; init; } = false;
	public string NetworkPathToPing { get; init; } = default!;
	public Dictionary<string, object> AdHocFields { get; init; } = default!;

	[JsonIgnore]
	public string Version { get; init; } = default!;
	[JsonIgnore]
	public string Build { get; init; } = default!;

	public List<ScheduledProcessConfiguration> ProcessesConfigurations { get; init; } = default!;

	public void ValidateServiceLevelConfig()
	{
		// Add service level config fields validations here
		// Most likely it will be HARD errors so 
		// throw new Exception("Error");
	}
}
