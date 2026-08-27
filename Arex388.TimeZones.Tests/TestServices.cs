using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace Arex388.TimeZones.Tests;

internal static class TestServices {
	//  ================================================================================
	//  Well-known instants (UTC)
	//  ================================================================================

	public static readonly DateTimeOffset January = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
	public static readonly DateTimeOffset July = new(2026, 7, 15, 12, 0, 0, TimeSpan.Zero);

	//  ================================================================================
	//  Factories
	//  ================================================================================

	public static ITimeZones CreateTimeZones(
		Action<TimeZonesOptions>? configure = null) => CreateProvider(configure).GetRequiredService<ITimeZones>();

	/// <summary>
	/// A service whose "now" is pinned to <paramref name="now"/> through a fake <see cref="TimeProvider"/>.
	/// </summary>
	public static ITimeZones CreateTimeZones(
		DateTimeOffset now) => new ServiceCollection()
		.AddSingleton<TimeProvider>(new FakeTimeProvider(now))
		.AddTimeZones()
		.BuildServiceProvider()
		.GetRequiredService<ITimeZones>();

	public static ServiceProvider CreateProvider(
		Action<TimeZonesOptions>? configure = null) => new ServiceCollection()
		.AddTimeZones(configure)
		.BuildServiceProvider();
}
