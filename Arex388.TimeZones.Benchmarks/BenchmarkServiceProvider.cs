using Microsoft.Extensions.DependencyInjection;

namespace Arex388.TimeZones.Benchmarks;

internal static class BenchmarkServiceProvider {
	/// <summary>
	/// The instant every at-time benchmark uses — a July noon, inside US and EU daylight savings.
	/// </summary>
	public static readonly DateTimeOffset At = new(2026, 7, 15, 12, 0, 0, TimeSpan.Zero);

	public static ITimeZones Create() => new ServiceCollection()
		.AddTimeZones()
		.BuildServiceProvider()
		.GetRequiredService<ITimeZones>();

	/// <summary>
	/// A service whose per-language catalog is already built, so the benchmark body measures only the call.
	/// </summary>
	public static ITimeZones CreateWarm() {
		var timeZones = Create();

		_ = timeZones.GetTimeZones(At);

		return timeZones;
	}
}
