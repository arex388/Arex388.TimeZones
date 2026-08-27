using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class CoordinateTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Theory]
	[InlineData(40.7128, -74.0060, "America/New_York")]
	[InlineData(36.0, -114.9, "America/Los_Angeles")]
	[InlineData(0.0, 0.0, "UTC")]
	[InlineData(90.0, 0.0, "UTC")]
	[InlineData(0.0, -30.0, "Etc/GMT+2")]
	[InlineData(0.0, 180.0, "Etc/GMT-12")]
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
		var byCoordinate = _timeZones.GetTimeZoneByCoordinate(new Coordinate(40.7128, -74.0060), TestServices.January);

		byDouble.Should().NotBeNull();
		byCoordinate.Should().Be(byDouble);
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
	[InlineData(90.000001, 0.0, "latitude")]
	[InlineData(-90.000001, 0.0, "latitude")]
	[InlineData(double.NaN, 0.0, "latitude")]
	[InlineData(double.PositiveInfinity, 0.0, "latitude")]
	[InlineData(double.NegativeInfinity, 0.0, "latitude")]
	[InlineData(0.0, 180.000001, "longitude")]
	[InlineData(0.0, -180.000001, "longitude")]
	[InlineData(0.0, double.NaN, "longitude")]
	[InlineData(0.0, double.PositiveInfinity, "longitude")]
	[InlineData(0.0, double.NegativeInfinity, "longitude")]
	public void GetTimeZoneByCoordinate_Doubles_OutsideBoundaries_Throw(
		double latitude,
		double longitude,
		string paramName) {
		var act = () => _timeZones.GetTimeZoneByCoordinate(latitude, longitude, TestServices.January);

		act.Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be(paramName);
	}

	[Theory]
	[InlineData(90.000001, 0.0)]
	[InlineData(double.NaN, 0.0)]
	[InlineData(0.0, -180.000001)]
	[InlineData(0.0, double.NegativeInfinity)]
	public void GetTimeZoneByCoordinate_ICoordinate_OutsideBoundaries_Throws(
		double latitude,
		double longitude) {
		var act = () => _timeZones.GetTimeZoneByCoordinate(new Coordinate(latitude, longitude), TestServices.January);

		act.Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("coordinate");
	}

	[Fact]
	public void GetTimeZoneByCoordinate_NullCoordinate_Throws() {
		var act = () => _timeZones.GetTimeZoneByCoordinate(null!, TestServices.January);

		act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("coordinate");
	}
}
