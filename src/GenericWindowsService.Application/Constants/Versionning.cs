using System.Reflection;
using System.Text.RegularExpressions;

namespace GenericWindowsService.Application.Constants;

public static class Versionning
{
	public const string ENV_DEFAULT_VARIABLE_ANSWER = "N/A";
	public const char INFORMATIONAL_VERSION_PLUS_SEPARATOR = '+';

	public static string GetVersion()
	{
		try
		{
			var roughVersion = typeof(Program).Assembly.GetName().Version?.ToString();
			var version = Regex
				.Match(roughVersion!, @"^([0-9]*.[0-9]*.[0-9]*)(?=.[0-9]*)")
				.ToString();
			return version;
		}
		catch (Exception)
		{
			return ENV_DEFAULT_VARIABLE_ANSWER;
		}
	}

	public static string GetVersionPostfix()
	{
		try
		{
			var informationalVersion = Assembly.GetEntryAssembly()!
				.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
				.InformationalVersion;

			var plusIndex = informationalVersion.IndexOf(INFORMATIONAL_VERSION_PLUS_SEPARATOR);
			if (plusIndex > 0)
			{
				return informationalVersion.Substring(0, plusIndex);
			}
			else
			{
				return informationalVersion;
			}
		}
		catch (Exception)
		{
			return ENV_DEFAULT_VARIABLE_ANSWER;
		}
	}
}
