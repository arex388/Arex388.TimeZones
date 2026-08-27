using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Arex388.TimeZones.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0, id: ".NET 10")]
public class ListBenchmarks {
	private ITimeZones _timeZones = null!;

	[GlobalSetup]
	public void Setup() => _timeZones = BenchmarkServiceProvider.CreateWarm();

	/// <summary>The full list as of now — a snapshot of every catalog entry at the clock's instant.</summary>
	[Benchmark]
	public IReadOnlyList<TimeZone> GetTimeZones() => _timeZones.GetTimeZones();

	/// <summary>The full list as of a fixed instant.</summary>
	[Benchmark]
	public IReadOnlyList<TimeZone> GetTimeZonesAt() => _timeZones.GetTimeZones(BenchmarkServiceProvider.At);

	/// <summary>The Eastern Standard Time fan-out (about twenty zones) at a fixed instant.</summary>
	[Benchmark]
	public IReadOnlyList<TimeZone> GetTimeZonesByWindowsId() => _timeZones.GetTimeZonesByWindowsId("Eastern Standard Time", BenchmarkServiceProvider.At);
}
