using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class IanaIdTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void GetTimeZoneByIanaId_Unknown_ReturnsNull() => _timeZones.GetTimeZoneByIanaId("Nowhere/Unknown", TestServices.January).Should().BeNull();

	[Fact]
	public void GetTimeZoneByIanaId_NoWindowsMapping_ReturnsNull() => _timeZones.GetTimeZoneByIanaId("Antarctica/Troll", TestServices.January).Should().BeNull();

	[Fact]
	public void GetTimeZoneByIanaId_Alias_ResolvesWindowsId() {
		var timeZone = _timeZones.GetTimeZoneByIanaId("US/Eastern", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.WindowsId.Should().Be("Eastern Standard Time");
	}

	[Fact]
	public void GetTimeZoneByIanaId_Utc() {
		var timeZone = _timeZones.GetTimeZoneByIanaId("UTC", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.IanaId.Should().Be("UTC");
		timeZone.WindowsId.Should().Be("UTC");
	}
}
