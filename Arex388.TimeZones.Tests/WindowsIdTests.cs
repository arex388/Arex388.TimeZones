using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class WindowsIdTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void GetTimeZonesByWindowsId_EasternStandardTime_FansOut() {
		var timeZones = _timeZones.GetTimeZonesByWindowsId("Eastern Standard Time", TestServices.January);

		timeZones.Select(tz => tz.IanaId).Should().Contain(["America/New_York", "America/Toronto", "US/Eastern"]);
		timeZones.Should().OnlyContain(tz => tz.WindowsId == "Eastern Standard Time");
		timeZones.Should().BeInAscendingOrder(tz => tz.IanaId, StringComparer.Ordinal);
	}

	[Fact]
	public void GetTimeZonesByWindowsId_Unknown_IsEmpty() => _timeZones.GetTimeZonesByWindowsId("Nowhere Standard Time", TestServices.January).Should().BeEmpty();

	[Fact]
	public void GetTimeZonesByWindowsId_Null_Throws() {
		var act = () => _timeZones.GetTimeZonesByWindowsId(null!, TestServices.January);

		act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("windowsId");
	}

	[Fact]
	public void GetTimeZoneByWindowsId_ReturnsCanonicalZone() {
		var timeZone = _timeZones.GetTimeZoneByWindowsId("Eastern Standard Time", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.IanaId.Should().Be("America/New_York");
	}

	[Fact]
	public void GetTimeZoneByWindowsId_Territory_ReturnsTerritoryZone() {
		var timeZone = _timeZones.GetTimeZoneByWindowsId("Eastern Standard Time", TestServices.January, "CA");

		timeZone.Should().NotBeNull();
		timeZone!.IanaId.Should().Be("America/Toronto");
	}

	[Fact]
	public void GetTimeZoneByWindowsId_UnmappedTerritory_FallsBackToDefault() => _timeZones.GetTimeZoneByWindowsId("Eastern Standard Time", TestServices.January, "ZZ")!.IanaId.Should().Be("America/New_York");

	[Fact]
	public void GetTimeZoneByWindowsId_IsCaseInsensitive() => _timeZones.GetTimeZoneByWindowsId("EASTERN STANDARD TIME", TestServices.January)!.IanaId.Should().Be("America/New_York");

	[Fact]
	public void GetTimeZoneByWindowsId_Unknown_ReturnsNull() => _timeZones.GetTimeZoneByWindowsId("Nowhere Standard Time", TestServices.January).Should().BeNull();

	[Fact]
	public void GetTimeZoneByWindowsId_Null_Throws() {
		var act = () => _timeZones.GetTimeZoneByWindowsId(null!, TestServices.January);

		act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("windowsId");
	}
}
