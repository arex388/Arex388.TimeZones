using Microsoft.Extensions.DependencyInjection;

namespace Arex388.TimeZones.Tests;

internal static class TestServices {
	public static ITimeZones CreateTimeZones() => new ServiceCollection()
		.AddMemoryCache()
		.AddTimeZones()
		.BuildServiceProvider()
		.GetRequiredService<ITimeZones>();

	//  ================================================================================
	//  Well-known instants (UTC)
	//  ================================================================================

	public static readonly DateTimeOffset January = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
	public static readonly DateTimeOffset July = new(2026, 7, 15, 12, 0, 0, TimeSpan.Zero);
}
