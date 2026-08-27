using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Arex388.TimeZones.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0, id: ".NET 10")]
public class LookupBenchmarks {
	private ITimeZones _timeZones = null!;

	[GlobalSetup]
	public void Setup() => _timeZones = BenchmarkServiceProvider.CreateWarm();

	/// <summary>A canonical id.</summary>
	[Benchmark(Baseline = true)]
	public TimeZone? ByIanaId() => _timeZones.GetTimeZoneByIanaId("America/New_York", BenchmarkServiceProvider.At);

	/// <summary>An alias — same path, the catalog carries aliases as entries.</summary>
	[Benchmark]
	public TimeZone? ByIanaIdAlias() => _timeZones.GetTimeZoneByIanaId("US/Eastern", BenchmarkServiceProvider.At);

	/// <summary>A differently-cased id — the ordinal-ignore-case dictionary, no normalization allocation.</summary>
	[Benchmark]
	public TimeZone? ByIanaIdCased() => _timeZones.GetTimeZoneByIanaId("america/new_york", BenchmarkServiceProvider.At);

	/// <summary>An unknown id — the miss path, no snapshot.</summary>
	[Benchmark]
	public TimeZone? ByIanaIdUnknown() => _timeZones.GetTimeZoneByIanaId("Nowhere/Unknown", BenchmarkServiceProvider.At);

	/// <summary>A Windows id for the default territory.</summary>
	[Benchmark]
	public TimeZone? ByWindowsId() => _timeZones.GetTimeZoneByWindowsId("Eastern Standard Time", BenchmarkServiceProvider.At);

	/// <summary>A Windows id for a territory.</summary>
	[Benchmark]
	public TimeZone? ByWindowsIdTerritory() => _timeZones.GetTimeZoneByWindowsId("Eastern Standard Time", BenchmarkServiceProvider.At, "CA");

	/// <summary>New York by coordinate — dominated by the GeoTimeZone lookup.</summary>
	[Benchmark]
	public TimeZone? ByCoordinate() => _timeZones.GetTimeZoneByCoordinate(40.7128, -74.0060, BenchmarkServiceProvider.At);
}
