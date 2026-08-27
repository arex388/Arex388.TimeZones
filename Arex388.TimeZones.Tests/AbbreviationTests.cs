using FluentAssertions;

namespace Arex388.TimeZones.Tests;

public sealed class AbbreviationTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void NewYork_IsEstInJanuaryAndEdtInJuly() {
		_timeZones.GetTimeZoneByIanaId("America/New_York", TestServices.January)!.Abbreviation.Should().Be("EST");
		_timeZones.GetTimeZoneByIanaId("America/New_York", TestServices.July)!.Abbreviation.Should().Be("EDT");
	}

	[Fact]
	public void Kolkata_IsIst() => _timeZones.GetTimeZoneByIanaId("Asia/Kolkata", TestServices.January)!.Abbreviation.Should().Be("IST");

	[Fact]
	public void EtcGmtPlus5_IsUtcMinus5() => _timeZones.GetTimeZoneByIanaId("Etc/GMT+5", TestServices.January)!.Abbreviation.Should().Be("UTC-5");

	[Fact]
	public void Berlin_DiffersBetweenEnglishAndGerman() {
		var english = _timeZones.GetTimeZoneByIanaId("Europe/Berlin", TestServices.January, "en")!.Abbreviation;
		var german = _timeZones.GetTimeZoneByIanaId("Europe/Berlin", TestServices.January, "de")!.Abbreviation;

		english.Should().Be("CET");
		german.Should().Be("MEZ");
	}
}
