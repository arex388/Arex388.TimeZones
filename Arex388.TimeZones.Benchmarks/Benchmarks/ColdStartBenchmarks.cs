using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;

namespace Arex388.TimeZones.Benchmarks;

/// <summary>
/// The one deliberately expensive path: the first full list for a fresh service builds the per-language
/// catalog (TimeZoneNames for about six hundred zones). Each invocation gets a new service so every
/// measurement is a real cold start.
/// </summary>
[MemoryDiagnoser]
[MedianColumn]
[SimpleJob(RunStrategy.Monitoring, RuntimeMoniker.Net10_0, id: ".NET 10", invocationCount: 1, iterationCount: 20)]
public class ColdStartBenchmarks {
	private ITimeZones _timeZones = null!;

	[IterationSetup]
	public void Setup() => _timeZones = BenchmarkServiceProvider.Create();

	/// <summary>The first GetTimeZones for a fresh service and the default language — the catalog build.</summary>
	[Benchmark]
	public IReadOnlyList<TimeZone> FirstGetTimeZones() => _timeZones.GetTimeZones(BenchmarkServiceProvider.At);
}
