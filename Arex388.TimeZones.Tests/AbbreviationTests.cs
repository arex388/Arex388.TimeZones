using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Arex388.TimeZones.Tests;

public sealed class AbbreviationTests {
	private readonly ITimeZones _timeZones = TestServices.CreateTimeZones();

	[Fact]
	public void NewYork_IsEstInJanuaryAndEdtInJuly() {
		_timeZones.GetTimeZoneByIanaId("America/New_York", TestServices.January)!.Abbreviation.Should().Be("EST");
		_timeZones.GetTimeZoneByIanaId("America/New_York", TestServices.July)!.Abbreviation.Should().Be("EDT");
	}

	[Fact]
	public void NewYork_NameIsDstAware() {
		_timeZones.GetTimeZoneByIanaId("America/New_York", TestServices.January)!.Name.Should().Be("Eastern Standard Time");
		_timeZones.GetTimeZoneByIanaId("America/New_York", TestServices.July)!.Name.Should().Be("Eastern Daylight Time");
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

	[Fact]
	public void BuenosAires_FallsBackToGenericAbbreviation() {
		var timeZone = _timeZones.GetTimeZoneByIanaId("America/Argentina/Buenos_Aires", TestServices.January);

		timeZone.Should().NotBeNull();
		timeZone!.Name.Should().Be("Argentina Standard Time");
		timeZone.Abbreviation.Should().Be("ART");
	}

	//	3.0.1 fell back to the zone's own IANA id ("America/Argentina/Buenos_Aires")
	//	when CLDR had no abbreviation; 4.0 never does. The legacy single-token zones
	//	(CET, GMT, UTC, …) are TZDB ids whose CLDR abbreviation is the same string,
	//	so the check is "no slash-qualified id", not "not any TZDB id".
	[Fact]
	public void EveryZone_HasANameAndNeverAnIanaIdOrEmptyAbbreviation() {
		foreach (var at in new[] { TestServices.January, TestServices.July }) {
			foreach (var timeZone in _timeZones.GetTimeZones(at)) {
				timeZone.Name.Should().NotBeNullOrEmpty(timeZone.IanaId);
				timeZone.Abbreviation.Should().NotBeEmpty(timeZone.IanaId);
				timeZone.Abbreviation.Should().NotContain("/", timeZone.IanaId);
			}
		}
	}

	[Fact]
	public void UnknownLanguageCode_ThrowsArgumentException() {
		var act = () => _timeZones.GetTimeZoneByIanaId("UTC", TestServices.January, "xx-XX");

		act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("languageCode");
	}

	[Fact]
	public void UnknownDefaultLanguageCode_FailsAtResolution() {
		using var provider = TestServices.CreateProvider(o => o.DefaultLanguageCode = "xx-XX");

		var act = () => provider.GetRequiredService<ITimeZones>();

		act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("options");
	}

	[Fact]
	public void DefaultLanguageCode_IsHonored() {
		var german = TestServices.CreateTimeZones(o => o.DefaultLanguageCode = "de");

		german.GetTimeZoneByIanaId("Europe/Berlin", TestServices.January)!.Abbreviation.Should().Be("MEZ");
	}
}
