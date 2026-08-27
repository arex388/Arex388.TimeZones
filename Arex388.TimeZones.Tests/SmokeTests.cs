using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class SmokeTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void GetTimeZones_Now_DoesNotThrow() => _timeZones.GetTimeZones().Should().NotBeEmpty();

	[Fact]
	public void GetTimeZoneByCoordinate_Now_DoesNotThrow() => _timeZones.GetTimeZoneByCoordinate(40.7128, -74.0060).Should().NotBeNull();

	[Fact]
	public void GetTimeZoneByIanaId_Now_DoesNotThrow() => _timeZones.GetTimeZoneByIanaId("America/New_York").Should().NotBeNull();

	[Fact]
	public void GetTimeZonesByWindowsId_Now_DoesNotThrow() => _timeZones.GetTimeZonesByWindowsId("Eastern Standard Time").Should().NotBeEmpty();

	[Fact]
	public void GetTimeZones_TwoCalls_ReturnEqualContents() {
		var first = _timeZones.GetTimeZones(TestServices.January).ToList();
		var second = _timeZones.GetTimeZones(TestServices.January).ToList();

		second.Should().BeEquivalentTo(first);
	}
}
