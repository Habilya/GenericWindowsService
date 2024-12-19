using System.Text.Json.Serialization;

namespace GenericWindowsService.Application.ServiceConfiguration;

public class ServiceConfiguration
{
	public ServiceConfiguration()
	{
		Version = typeof(Program).Assembly.GetName().Version?.ToString() ?? Constants.Config.ENV_DEFAULT_VARIABLE_ANSWER;
		Build = Environment.GetEnvironmentVariable(Constants.Config.ENV_BUILD_NUMBER) ?? Constants.Config.ENV_DEFAULT_VARIABLE_ANSWER;
		Environement = Environment.GetEnvironmentVariable(Constants.Config.ENV_DOTNET_ENVIRONMENT) ?? Constants.Config.ENV_DEFAULT_VARIABLE_ANSWER;
	}

	public string ServiceName { get; init; } = default!;
	public int RunEveryMS { get; init; } = 3000;
	public int MaxDegreeOfParallelism { get; init; } = 3;
	public bool IsNetworkPingEnabled { get; init; } = false;
	public string NetworkPathToPing { get; init; } = default!;
	public Dictionary<string, object> AdHocFields { get; init; } = default!;

	[JsonIgnore]
	public string Version { get; init; } = default!;
	[JsonIgnore]
	public string Build { get; init; } = default!;
	[JsonIgnore]
	public string Environement { get; init; } = default!;

	public List<ScheduledProcessConfiguration> ProcessesConfigurations { get; init; } = default!;

	public void ValidateServiceLevelConfig()
	{
		// Add service level config fields validations here
		// Most likely it will be HARD errors so 
		// throw new Exception("Error");
	}
}
