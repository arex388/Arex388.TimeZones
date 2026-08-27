using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class CoordinateTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Theory]
	[InlineData(40.7128, -74.0060, "America/New_York")]
	[InlineData(36.0, -114.9, "America/Los_Angeles")]
	[InlineData(0.0, 0.0, "UTC")]
	[InlineData(90.0, 0.0, "UTC")]
	[InlineData(-90.0, 180.0, "Antarctica/McMurdo")]
	public void GetTimeZoneByCoordinate_ResolvesExpectedZone(
		double latitude,
		double longitude,
		string ianaId) {
		var timeZone = _timeZones.GetTimeZoneByCoordinate(latitude, longitude, TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.IanaId.Should().Be(ianaId);
	}

	[Fact]
	public void GetTimeZoneByCoordinate_Overloads_Agree() {
		var byDouble = _timeZones.GetTimeZoneByCoordinate(40.7128, -74.0060, TestServices.January);
		var byDecimal = _timeZones.GetTimeZoneByCoordinate(40.7128M, -74.0060M, TestServices.January);
		var byCoordinate = _timeZones.GetTimeZoneByCoordinate(new Coordinate {
			Latitude = 40.7128,
			Longitude = -74.0060
		}, TestServices.January);

		byDouble.Should().NotBeNull();
		byDecimal.Should().BeEquivalentTo(byDouble);
		byCoordinate.Should().BeEquivalentTo(byDouble);
	}

	[Theory]
	[InlineData(90.0, 0.0)]
	[InlineData(-90.0, 0.0)]
	[InlineData(0.0, 180.0)]
	[InlineData(0.0, -180.0)]
	public void GetTimeZoneByCoordinate_InclusiveBoundaries_DoNotThrow(
		double latitude,
		double longitude) {
		var act = () => _timeZones.GetTimeZoneByCoordinate(latitude, longitude, TestServices.January);

		act.Should().NotThrow();
	}

	[Theory]
	[InlineData(90.000001, 0.0)]
	[InlineData(-90.000001, 0.0)]
	[InlineData(0.0, 180.000001)]
	[InlineData(0.0, -180.000001)]
	public void GetTimeZoneByCoordinate_OutsideBoundaries_Throws(
		double latitude,
		double longitude) {
		var act = () => _timeZones.GetTimeZoneByCoordinate(latitude, longitude, TestServices.January);

		act.Should().Throw<ArgumentOutOfRangeException>();
	}
}
