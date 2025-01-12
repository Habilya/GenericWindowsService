﻿using GenericWindowsService.Application.ServiceConfiguration;
using GenericWindowsService.Library.Logging;

namespace GenericWindowsService.Application.Process;

public class GenericProcess : IGenericProcess
{
	protected readonly ServiceGenericConfiguration _serviceConfiguration;
	protected readonly ILoggerAdapter<GenericProcess> _loggerAdapter;

	public ScheduledProcessConfiguration ScheduledProcessConfiguration { protected set; get; } = default!;
	public bool IsValid { get; set; } = default!;
	public List<string> ValidationMessages { get; set; } = new List<string>();
	public DateTime NextRunTime { get; set; } = DateTime.MinValue;

	public GenericProcess(
		ILoggerAdapter<GenericProcess> loggerAdapter,
		ServiceGenericConfiguration serviceConfiguration)
	{
		_serviceConfiguration = serviceConfiguration;
		_loggerAdapter = loggerAdapter;
	}

	public virtual void InitConfig(ScheduledProcessConfiguration config)
	{
		if (ScheduledProcessConfiguration == default!)
		{
			ScheduledProcessConfiguration = config;
		}
		else
		{
			ValidationMessages.Add("Config can only be initialized once..");
			LogWarning("Attempt to reinitialize config.");
		}
	}

	public virtual void ValidateConfig()
	{
		// In the inhertided classes vrite some validations to populate ValidationMessages
		if (ValidationMessages.Any())
		{
			IsValid = false;
		}
	}

	public virtual bool IsValidBeforeRun()
	{
		// In the inhertided classes vrite some validations to populate ValidationMessages
		return true;
	}

	// Wrappers of the log methods
	protected virtual void LogInformation(string? message, params object?[] args)
	{
		_loggerAdapter.LogInformation(PrependProcessNameToMessage(message), args);
	}

	protected virtual void LogWarning(string? message, params object?[] args)
	{
		_loggerAdapter.LogWarning(PrependProcessNameToMessage(message), args);
	}

	protected virtual void LogError(Exception? exception, string? message, params object?[] args)
	{
		_loggerAdapter.LogError(exception, PrependProcessNameToMessage(message), args);
	}

	protected virtual string? PrependProcessNameToMessage(string? message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return message;
		}

		return $"[{ScheduledProcessConfiguration.LabelAndProcessCode}]: {message}";
	}

	public virtual void RunProcess() { }
}
