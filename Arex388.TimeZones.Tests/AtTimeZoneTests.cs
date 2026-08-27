using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class AtTimeZoneTests {
	private static readonly DateTimeOffset _value = new(2026, 3, 8, 9, 30, 15, TimeSpan.FromHours(-7));

	[Theory]
	[InlineData(120)]
	[InlineData(-300)]
	[InlineData(330)]
	public void AtTimeZone_KeepsWallClock_SwapsOffset(
		int minutes) {
		var offset = TimeSpan.FromMinutes(minutes);
		var result = _value.AtTimeZone(offset);

		result.Year.Should().Be(_value.Year);
		result.Month.Should().Be(_value.Month);
		result.Day.Should().Be(_value.Day);
		result.Hour.Should().Be(_value.Hour);
		result.Minute.Should().Be(_value.Minute);
		result.Second.Should().Be(_value.Second);
		result.Offset.Should().Be(offset);
	}

	[Fact]
	public void AtTimeZone_Nullable_MapsNullToNull() {
		DateTimeOffset? value = null;

		value.AtTimeZone(TimeSpan.FromHours(1)).Should().BeNull();
	}

	[Fact]
	public void AtTimeZone_Nullable_LiftsValue() {
		DateTimeOffset? value = _value;

		value.AtTimeZone(TimeSpan.FromHours(1)).Should().Be(_value.AtTimeZone(TimeSpan.FromHours(1)));
	}
}
