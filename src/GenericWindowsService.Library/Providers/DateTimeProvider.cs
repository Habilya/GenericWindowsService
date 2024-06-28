namespace GenericWindowsService.Library.Providers;

public class DateTimeProvider : IDateTimeProvider
{
	public DateTime DateTimeNow => DateTime.Now;
}
