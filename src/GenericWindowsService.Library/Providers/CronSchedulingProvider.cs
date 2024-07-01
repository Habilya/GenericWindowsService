using NCrontab;

namespace GenericWindowsService.Library.Providers;

public class CronSchedulingProvider : ICronSchedulingProvider
{
	private readonly IDateTimeProvider _dateTimeProvider;

	public CronSchedulingProvider(IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
	}

	public DateTime GetNextRunFromSchedule(string cronSchedule)
	{
		var scheduling = CrontabSchedule.Parse(cronSchedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
		return scheduling.GetNextOccurrence(_dateTimeProvider.DateTimeNow);
	}
}
