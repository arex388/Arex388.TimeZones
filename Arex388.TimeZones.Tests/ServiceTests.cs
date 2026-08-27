using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.TimeZones;

namespace Arex388.TimeZones.Tests;

public sealed class ServiceTests {
	//  ================================================================================
	//  "Now" through a fake TimeProvider
	//  ================================================================================

	[Fact]
	public void NowOverloads_UseTheRegisteredTimeProvider() {
		var january = TestServices.CreateTimeZones(TestServices.January);
		var july = TestServices.CreateTimeZones(TestServices.July);

		january.GetTimeZoneByIanaId("America/New_York")!.UtcOffset.Should().Be(TimeSpan.FromHours(-5));
		july.GetTimeZoneByIanaId("America/New_York")!.UtcOffset.Should().Be(TimeSpan.FromHours(-4));
		january.GetTimeZoneByIanaId("America/New_York")!.At.Should().Be(TestServices.January);

		july.GetTimeZones().Should().BeEquivalentTo(july.GetTimeZones(TestServices.July));
		july.GetTimeZoneByCoordinate(40.7128, -74.0060).Should().Be(july.GetTimeZoneByCoordinate(40.7128, -74.0060, TestServices.July));
		july.GetTimeZoneByCoordinate(new Coordinate(40.7128, -74.0060)).Should().Be(july.GetTimeZoneByCoordinate(new Coordinate(40.7128, -74.0060), TestServices.July));
		july.GetTimeZoneByWindowsId("Eastern Standard Time").Should().Be(july.GetTimeZoneByWindowsId("Eastern Standard Time", TestServices.July));
		july.GetTimeZonesByWindowsId("Eastern Standard Time").Should().BeEquivalentTo(july.GetTimeZonesByWindowsId("Eastern Standard Time", TestServices.July));
	}

	[Fact]
	public void NowOverloads_WithoutATimeProvider_UseTheSystemClock() {
		var timeZones = TestServices.CreateTimeZones();
		var before = DateTimeOffset.UtcNow;
		var timeZone = timeZones.GetTimeZoneByIanaId("UTC");
		var after = DateTimeOffset.UtcNow;

		timeZone.Should().NotBeNull();
		timeZone!.At.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
	}

	//  ================================================================================
	//  Snapshots are independent
	//  ================================================================================

	[Fact]
	public void Results_AreIndependentSnapshots() {
		var timeZones = TestServices.CreateTimeZones();
		var first = timeZones.GetTimeZones(TestServices.January);
		var second = timeZones.GetTimeZones(TestServices.January);

		second.Should().Equal(first);
		second.Should().NotBeSameAs(first);

		((TimeZone[])first).AsSpan().Clear();

		timeZones.GetTimeZones(TestServices.January).Should().Equal(second);
		timeZones.GetTimeZones(TestServices.January).Should().HaveCount(596);
	}

	//  ================================================================================
	//  Registration
	//  ================================================================================

	[Fact]
	public void AddTimeZones_Twice_RegistersOnce() {
		var services = new ServiceCollection().AddTimeZones().AddTimeZones();

		services.Count(sd => sd.ServiceType == typeof(ITimeZones)).Should().Be(1);
		services.BuildServiceProvider().GetServices<ITimeZones>().Should().ContainSingle();
	}

	[Fact]
	public void AddTimeZones_IsASingleton() {
		using var provider = TestServices.CreateProvider();

		provider.GetRequiredService<ITimeZones>().Should().BeSameAs(provider.GetRequiredService<ITimeZones>());
	}

	[Fact]
	public void DateTimeZoneProvider_Override_IsHonored() {
		using var stream = typeof(TzdbDateTimeZoneSource).Assembly.GetManifestResourceStream("NodaTime.TimeZones.Tzdb.nzd")!;

		var source = TzdbDateTimeZoneSource.FromStream(stream);
		var custom = new DateTimeZoneCache(source);
		var timeZones = TestServices.CreateTimeZones(o => o.DateTimeZoneProvider = custom);

		timeZones.TzdbVersion.Should().Be(custom.VersionId);
		timeZones.GetTimeZoneByIanaId("US/Eastern", TestServices.January)!.CanonicalIanaId.Should().Be("America/New_York");
		timeZones.GetTimeZones(TestServices.January).Should().HaveCount(596);
	}
}
