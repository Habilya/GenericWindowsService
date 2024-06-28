namespace GenericWindowsService.Library.Providers;

public interface IDateTimeProvider
{
	DateTime DateTimeNow { get; }
}
