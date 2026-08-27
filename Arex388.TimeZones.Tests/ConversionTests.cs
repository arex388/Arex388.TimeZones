using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class ConversionTests {
	private const string NewYork = "America/New_York";

	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	//  ================================================================================
	//  AtTimeZone — wall clock preserved
	//  ================================================================================

	//	2026-03-08 is the US spring-forward (02:00 → 03:00); 2026-11-01 is the
	//	fall-back (02:00 → 01:00). Wall clock in, expected wall clock and offset out.
	[Theory]
	[InlineData("2026-03-08T00:00:00", "2026-03-08T00:00:00", -300, TimeZoneResolution.Unambiguous)]
	[InlineData("2026-03-08T02:30:00", "2026-03-08T03:30:00", -240, TimeZoneResolution.Skipped)]
	[InlineData("2026-03-08T03:00:00", "2026-03-08T03:00:00", -240, TimeZoneResolution.Unambiguous)]
	[InlineData("2026-11-01T00:00:00", "2026-11-01T00:00:00", -240, TimeZoneResolution.Unambiguous)]
	[InlineData("2026-11-01T01:30:00", "2026-11-01T01:30:00", -240, TimeZoneResolution.Ambiguous)]
	[InlineData("2026-11-01T02:00:00", "2026-11-01T02:00:00", -300, TimeZoneResolution.Unambiguous)]
	public void TryAtTimeZone_NewYork_AcrossDst(
		string wallClock,
		string expectedWallClock,
		int expectedMinutes,
		TimeZoneResolution expectedResolution) {
		var input = DateTime.Parse(wallClock, System.Globalization.CultureInfo.InvariantCulture);
		var expected = new DateTimeOffset(DateTime.Parse(expectedWallClock, System.Globalization.CultureInfo.InvariantCulture), TimeSpan.FromMinutes(expectedMinutes));

		_timeZones.TryAtTimeZone(input, NewYork, out var result, out var resolution).Should().BeTrue();

		result.Should().Be(expected);
		result.Offset.Should().Be(expected.Offset);
		resolution.Should().Be(expectedResolution);
		_timeZones.AtTimeZone(input, NewYork).Should().Be(expected);
	}

	[Theory]
	[InlineData(DateTimeKind.Unspecified)]
	[InlineData(DateTimeKind.Utc)]
	[InlineData(DateTimeKind.Local)]
	public void AtTimeZone_DateTime_KindHasNoEffect(
		DateTimeKind kind) {
		var wallClock = new DateTime(2026, 7, 15, 9, 0, 0, kind);
		var result = _timeZones.AtTimeZone(wallClock, NewYork);

		result.Should().Be(new DateTimeOffset(2026, 7, 15, 9, 0, 0, TimeSpan.FromHours(-4)));
		result!.Value.Offset.Should().Be(TimeSpan.FromHours(-4));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-420)]
	[InlineData(540)]
	public void AtTimeZone_DateTimeOffset_InputOffsetHasNoEffect(
		int inputMinutes) {
		var wallClock = new DateTimeOffset(2026, 1, 15, 9, 0, 0, TimeSpan.FromMinutes(inputMinutes));
		var result = _timeZones.AtTimeZone(wallClock, NewYork);

		result.Should().NotBeNull();
		result!.Value.DateTime.Should().Be(wallClock.DateTime);
		result.Value.Offset.Should().Be(TimeSpan.FromHours(-5));
	}

	[Fact]
	public void AtTimeZone_UnknownZone_ReturnsNullAndFalse() {
		_timeZones.AtTimeZone(new DateTime(2026, 1, 1), "Nowhere/Unknown").Should().BeNull();
		_timeZones.TryAtTimeZone(new DateTime(2026, 1, 1), "Nowhere/Unknown", out _, out _).Should().BeFalse();
	}

	[Fact]
	public void AtTimeZone_NullIanaId_Throws() {
		var act = () => _timeZones.AtTimeZone(new DateTime(2026, 1, 1), null!);

		act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("ianaId");
	}

	//  ================================================================================
	//  InTimeZone — instant preserved
	//  ================================================================================

	[Fact]
	public void InTimeZone_Kolkata_ShowsTheSameInstant() {
		var result = _timeZones.InTimeZone(TestServices.July, "Asia/Kolkata");

		result.Should().Be(TestServices.July);
		result!.Value.Offset.Should().Be(new TimeSpan(5, 30, 0));
		result.Value.DateTime.Should().Be(new DateTime(2026, 7, 15, 17, 30, 0));
	}

	[Fact]
	public void InTimeZone_RoundTripsThroughUtc() {
		var local = new DateTimeOffset(2026, 7, 15, 9, 0, 0, TimeSpan.FromHours(-4));
		var utc = _timeZones.InTimeZone(local, "UTC");
		var back = _timeZones.InTimeZone(utc!.Value, NewYork);

		utc.Value.Offset.Should().Be(TimeSpan.Zero);
		back.Should().Be(local);
		back!.Value.Offset.Should().Be(local.Offset);
	}

	[Fact]
	public void InTimeZone_UnknownZone_ReturnsNull() => _timeZones.InTimeZone(TestServices.July, "Nowhere/Unknown").Should().BeNull();

	//  ================================================================================
	//  Extensions
	//  ================================================================================

	[Fact]
	public void NullableLiftingOverloads_MapNullToNull() {
		DateTime? dateTime = null;
		DateTimeOffset? dateTimeOffset = null;

		_timeZones.AtTimeZone(dateTime, NewYork).Should().BeNull();
		_timeZones.AtTimeZone(dateTimeOffset, NewYork).Should().BeNull();
		_timeZones.InTimeZone(dateTimeOffset, NewYork).Should().BeNull();
		dateTimeOffset.AtTimeZone(TimeSpan.FromHours(1)).Should().BeNull();
	}

	[Fact]
	public void NullableLiftingOverloads_ForwardValues() {
		DateTime? dateTime = new DateTime(2026, 7, 15, 9, 0, 0);
		DateTimeOffset? dateTimeOffset = new DateTimeOffset(2026, 7, 15, 9, 0, 0, TimeSpan.Zero);

		_timeZones.AtTimeZone(dateTime, NewYork).Should().Be(_timeZones.AtTimeZone(dateTime.Value, NewYork));
		_timeZones.AtTimeZone(dateTimeOffset, NewYork).Should().Be(_timeZones.AtTimeZone(dateTimeOffset.Value, NewYork));
		_timeZones.InTimeZone(dateTimeOffset, NewYork).Should().Be(_timeZones.InTimeZone(dateTimeOffset.Value, NewYork));
		dateTimeOffset.AtTimeZone(TimeSpan.FromHours(1)).Should().Be(dateTimeOffset.Value.AtTimeZone(TimeSpan.FromHours(1)));
	}

	[Theory]
	[InlineData(120)]
	[InlineData(-300)]
	[InlineData(330)]
	public void AtTimeZone_TimeSpan_KeepsWallClockToTheTick_SwapsOffset(
		int minutes) {
		var value = new DateTimeOffset(2026, 3, 8, 9, 30, 15, TimeSpan.FromHours(-7)).AddTicks(1234567);
		var offset = TimeSpan.FromMinutes(minutes);
		var result = value.AtTimeZone(offset);

		result.DateTime.Should().Be(value.DateTime);
		result.Ticks.Should().Be(value.Ticks);
		result.Offset.Should().Be(offset);
	}
}
