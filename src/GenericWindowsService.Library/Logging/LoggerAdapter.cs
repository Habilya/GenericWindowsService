﻿using Microsoft.Extensions.Logging;

namespace GenericWindowsService.Library.Logging;

public class LoggerAdapter<TType> : ILoggerAdapter<TType>
{
	private readonly ILogger<TType> _logger;

	public LoggerAdapter(ILogger<TType> logger)
	{
		_logger = logger;
	}

	public void LogInformation(string? message, params object?[] args)
	{
		_logger.LogInformation(message, args);
	}

	public void LogWarning(string? message, params object?[] args)
	{
		_logger.LogWarning(message, args);
	}

	public void LogError(Exception? exception, string? message, params object?[] args)
	{
		_logger.LogError(exception, message, args);
	}
}
