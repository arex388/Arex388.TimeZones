using FluentAssertions;
using NodaTime;

namespace Arex388.TimeZones.Tests;

//	These pin the data the library ships with (TZDB 2025b via NodaTime 3.2.2,
//	TimeZoneConverter 7.1.0). A package bump that changes the data fails here
//	with the new numbers in the message, so the delta is reviewed rather than
//	shipped silently.
public sealed class DataPinningTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void GetTimeZones_CountIsExact() => _timeZones.GetTimeZones(TestServices.January).Should().HaveCount(596);

	[Fact]
	public void GetTimeZones_NoEmptyWindowsId() => _timeZones.GetTimeZones(TestServices.January).Should().OnlyContain(tz => !string.IsNullOrEmpty(tz.WindowsId));

	[Fact]
	public void GetTimeZones_OnlyTrollIsAbsent() {
		var ianaIds = _timeZones.GetTimeZones(TestServices.January).Select(tz => tz.IanaId).ToHashSet(StringComparer.Ordinal);
		var missing = DateTimeZoneProviders.Tzdb.Ids.Where(id => !ianaIds.Contains(id)).ToList();

		missing.Should().Equal("Antarctica/Troll");
	}

	[Fact]
	public void GetTimeZones_IanaIdsAreUnique() => _timeZones.GetTimeZones(TestServices.January).Select(tz => tz.IanaId).Should().OnlyHaveUniqueItems();
}
