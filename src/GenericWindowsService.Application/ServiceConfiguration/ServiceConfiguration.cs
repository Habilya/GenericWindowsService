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
	public int MaxDegreeOfParallelism { get; init; } = 5;
	public bool IsNetworkPingEnabled { get; init; } = false;
	public string NetworkPathToPing { get; init; } = default!;

	[JsonIgnore]
	public string Version { get; init; } = default!;
	[JsonIgnore]
	public string Build { get; init; } = default!;
	[JsonIgnore]
	public string Environement { get; init; } = default!;

	public List<ScheduledProcessConfiguration> ScheduledProcesses { get; init; } = default!;
}
