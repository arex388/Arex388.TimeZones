using System.Globalization;
using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class DaylightSavingsTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	//	Offsets in minutes at 2026-01-15T12:00Z and 2026-07-15T12:00Z, with the
	//	user-facing IsDaylightSavings flag for each. Europe/Dublin and
	//	Africa/Casablanca are the negative-DST zones: TZDB models their winter as
	//	negative savings, and the library normalizes that so summer is DST.
	//	Africa/Windhoek used negative DST until 2017 and is on permanent +02 since;
	//	its unbounded final interval follows the last negative winter and must not
	//	be flagged.
	[Theory]
	[InlineData("America/New_York", -300, false, -240, true)]
	[InlineData("Europe/London", 0, false, 60, true)]
	[InlineData("Europe/Dublin", 0, false, 60, true)]
	[InlineData("Africa/Windhoek", 120, false, 120, false)]
	[InlineData("Australia/Sydney", 660, true, 600, false)]
	[InlineData("Asia/Kolkata", 330, false, 330, false)]
	[InlineData("Asia/Kathmandu", 345, false, 345, false)]
	[InlineData("Australia/Lord_Howe", 660, true, 630, false)]
	[InlineData("Africa/Casablanca", 60, true, 60, true)]
	[InlineData("UTC", 0, false, 0, false)]
	public void GetTimeZoneByIanaId_OffsetAndDst(
		string ianaId,
		int januaryMinutes,
		bool januaryDst,
		int julyMinutes,
		bool julyDst) {
		var january = _timeZones.GetTimeZoneByIanaId(ianaId, TestServices.January);
		var july = _timeZones.GetTimeZoneByIanaId(ianaId, TestServices.July);

		january.Should().NotBeNull();
		july.Should().NotBeNull();

		january!.UtcOffset.Should().Be(TimeSpan.FromMinutes(januaryMinutes));
		july!.UtcOffset.Should().Be(TimeSpan.FromMinutes(julyMinutes));
		january.IsDaylightSavings.Should().Be(januaryDst);
		july.IsDaylightSavings.Should().Be(julyDst);
	}

	[Theory]
	[InlineData("2026-03-08T06:59:59Z", -300)]
	[InlineData("2026-03-08T07:00:00Z", -240)]
	[InlineData("2026-11-01T05:59:59Z", -240)]
	[InlineData("2026-11-01T06:00:00Z", -300)]
	public void GetTimeZoneByIanaId_NewYork_TransitionInstants(
		string instant,
		int minutes) {
		var at = DateTimeOffset.Parse(instant, CultureInfo.InvariantCulture);
		var timeZone = _timeZones.GetTimeZoneByIanaId("America/New_York", at);

		timeZone.Should().NotBeNull();
		timeZone!.UtcOffset.Should().Be(TimeSpan.FromMinutes(minutes));
	}

	[Fact]
	public void GetTimeZoneByIanaId_At_OffsetDoesNotChangeTheInstant() {
		var utc = new DateTimeOffset(2026, 3, 8, 7, 0, 0, TimeSpan.Zero);
		var foreign = utc.ToOffset(TimeSpan.FromHours(9));

		_timeZones.GetTimeZoneByIanaId("America/New_York", foreign).Should().Be(_timeZones.GetTimeZoneByIanaId("America/New_York", utc));
	}
}
