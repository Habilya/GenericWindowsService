using FluentAssertions;
using GenericWindowsService.Library.Providers;
using NSubstitute;
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
	[InlineData(1, "*/25 * * * * *", "2020-02-02 20:00:00", "2020-02-02 20:00:25")]
	[InlineData(2, "0 0 */12 * * *", "2020-02-02 20:00:00", "2020-02-03 00:00:00")]
	[InlineData(3, "0 */5 * * * *", "2020-02-02 20:00:00", "2020-02-02 20:05:00")]
	[InlineData(4, "0 0 12 * */2 Mon", "2020-02-02 20:00:00", "2020-03-02 12:00:00")]
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
		actual.Should().Be(expectedDateTime);
	}
}
