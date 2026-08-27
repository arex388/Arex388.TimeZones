using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class IanaIdTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void GetTimeZoneByIanaId_Unknown_ReturnsNull() => _timeZones.GetTimeZoneByIanaId("Nowhere/Unknown", TestServices.January).Should().BeNull();

	[Fact]
	public void GetTimeZoneByIanaId_NoWindowsMapping_ReturnsNull() => _timeZones.GetTimeZoneByIanaId("Antarctica/Troll", TestServices.January).Should().BeNull();

	[Fact]
	public void GetTimeZoneByIanaId_Null_Throws() {
		var act = () => _timeZones.GetTimeZoneByIanaId(null!, TestServices.January);

		act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("ianaId");
	}

	[Fact]
	public void GetTimeZoneByIanaId_Alias_ResolvesWindowsIdAndCanonical() {
		var timeZone = _timeZones.GetTimeZoneByIanaId("US/Eastern", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.IanaId.Should().Be("US/Eastern");
		timeZone.CanonicalIanaId.Should().Be("America/New_York");
		timeZone.IsAlias.Should().BeTrue();
		timeZone.WindowsId.Should().Be("Eastern Standard Time");
	}

	[Fact]
	public void GetTimeZoneByIanaId_Canonical_IsNotAlias() {
		var timeZone = _timeZones.GetTimeZoneByIanaId("America/New_York", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.CanonicalIanaId.Should().Be("America/New_York");
		timeZone.IsAlias.Should().BeFalse();
	}

	[Fact]
	public void GetTimeZoneByIanaId_IsCaseInsensitive_ReturnsTzdbCasing() {
		var timeZone = _timeZones.GetTimeZoneByIanaId("america/new_york", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.IanaId.Should().Be("America/New_York");
	}

	[Fact]
	public void GetTimeZoneByIanaId_Utc() {
		var timeZone = _timeZones.GetTimeZoneByIanaId("UTC", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.IanaId.Should().Be("UTC");
		timeZone.WindowsId.Should().Be("UTC");
	}

	[Fact]
	public void GetTimeZoneByIanaId_At_IsTheRequestedInstant() {
		var at = new DateTimeOffset(2026, 3, 8, 6, 59, 59, TimeSpan.FromHours(-7));
		var timeZone = _timeZones.GetTimeZoneByIanaId("America/New_York", at);

		timeZone.Should().NotBeNull();
		timeZone!.At.Should().Be(at);
		timeZone.At.Offset.Should().Be(TimeSpan.Zero);
	}
}
