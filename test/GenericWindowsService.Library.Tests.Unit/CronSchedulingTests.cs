using GenericWindowsService.Library.Providers;
using NSubstitute;
using Shouldly;
using System.Globalization;

namespace GenericWindowsService.Library.Tests.Unit;

public class CronSchedulingTests
{
	private readonly CronSchedulingProvider _sut;
	private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

	public CronSchedulingTests()
	{
		_sut = new CronSchedulingProvider(_dateTimeProvider);
	}

	[Theory]
	// runs every 25 seconds
	[InlineData(1, "*/25 * * * * *", "2020-02-02 20:00:00", "2020-02-02 20:00:25")]
	// runs every 5 minutes at 00 seconds
	[InlineData(2, "0 */5 * * * *", "2020-02-02 20:00:00", "2020-02-02 20:05:00")]
	// runs every 12 hours, (mind day is divided in 12 hours, so runs will be at 00:00:00 and 12:00:00)
	[InlineData(3, "0 0 */12 * * *", "2020-02-02 20:00:00", "2020-02-03 00:00:00")]
	// runs every day at 17:30:00
	[InlineData(4, "0 30 17 * * *", "2020-02-02 20:00:00", "2020-02-03 17:30:00")]
	// runs every Tuesday at 18:00
	[InlineData(5, "0 0 18 * * Tue", "2020-02-02 20:00:00", "2020-02-04 18:00:00")]
	// runs 12:00pm (noon) on Mondays of every other month starting from january
	[InlineData(6, "0 0 12 * */2 Mon", "2020-02-02 20:00:00", "2020-03-02 12:00:00")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "UnitTests with testId")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "UnitTests with testId")]
	public void GetNextRun_ShouldReturnDateTimeOfNextRun_ScheduleIsValid(int id, string schedule, string currentDate, string expected)
	{
		// Arrange
		const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
		_dateTimeProvider.DateTimeNow
			.Returns(DateTime.ParseExact(currentDate, DATE_TIME_FORMAT, CultureInfo.InvariantCulture));

		var expectedDateTime = DateTime.ParseExact(expected, DATE_TIME_FORMAT, CultureInfo.InvariantCulture);

		// Act
		var actual = _sut.GetNextRunFromSchedule(schedule);

		// Assert
		actual.ShouldBe(expectedDateTime);
	}
}
