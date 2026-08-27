using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class WindowsIdTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void GetTimeZonesByWindowsId_EasternStandardTime_FansOut() {
		var timeZones = _timeZones.GetTimeZonesByWindowsId("Eastern Standard Time", TestServices.January).ToList();

		timeZones.Select(tz => tz.IanaId).Should().Contain(["America/New_York", "America/Toronto", "US/Eastern"]);
		timeZones.Should().OnlyContain(tz => tz.WindowsId == "Eastern Standard Time");
	}

	[Fact]
	public void GetTimeZonesByWindowsId_Unknown_IsEmpty() => _timeZones.GetTimeZonesByWindowsId("Nowhere Standard Time", TestServices.January).Should().BeEmpty();
}
