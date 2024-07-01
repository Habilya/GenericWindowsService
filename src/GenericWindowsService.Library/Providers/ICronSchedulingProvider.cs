
namespace GenericWindowsService.Library.Providers;

public interface ICronSchedulingProvider
{
	DateTime GetNextRunFromSchedule(string cronSchedule);
}
