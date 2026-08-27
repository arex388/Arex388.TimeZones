using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Arex388.TimeZones.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0, id: ".NET 10")]
public class ConversionBenchmarks {
	private const string NewYork = "America/New_York";

	private static readonly DateTime _wallClock = new(2026, 7, 15, 9, 0, 0);
	private static readonly DateTime _ambiguousWallClock = new(2026, 11, 1, 1, 30, 0);
	private static readonly TimeSpan _offset = TimeSpan.FromHours(-4);

	private ITimeZones _timeZones = null!;
	private DateTimeOffset _wallClockOffset;

	[GlobalSetup]
	public void Setup() {
		_timeZones = BenchmarkServiceProvider.CreateWarm();
		_wallClockOffset = new DateTimeOffset(_wallClock, TimeSpan.Zero);
	}

	/// <summary>Wall clock preserved, from a DateTime.</summary>
	[Benchmark(Baseline = true)]
	public DateTimeOffset? AtTimeZoneDateTime() => _timeZones.AtTimeZone(_wallClock, NewYork);

	/// <summary>Wall clock preserved, from a DateTimeOffset whose offset is discarded.</summary>
	[Benchmark]
	public DateTimeOffset? AtTimeZoneDateTimeOffset() => _timeZones.AtTimeZone(_wallClockOffset, NewYork);

	/// <summary>The fall-back hour — MapLocal returns two candidates and the resolution is reported.</summary>
	[Benchmark]
	public TimeZoneResolution TryAtTimeZoneAmbiguous() {
		_timeZones.TryAtTimeZone(_ambiguousWallClock, NewYork, out _, out var resolution);

		return resolution;
	}

	/// <summary>Instant preserved.</summary>
	[Benchmark]
	public DateTimeOffset? InTimeZone() => _timeZones.InTimeZone(BenchmarkServiceProvider.At, NewYork);

	/// <summary>The offset swap — no zone, no service.</summary>
	[Benchmark]
	public DateTimeOffset AtTimeZoneTimeSpan() => _wallClockOffset.AtTimeZone(_offset);
}
