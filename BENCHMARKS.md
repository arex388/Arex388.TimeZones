# BENCHMARKS

#### 2026-08-26 (v4.0.0 baseline)

- BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
- Intel Core i7-4790K CPU 4.00GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
- .NET SDK 10.0.400
  - [Host]  : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3
  - .NET 10 : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

First baseline over the 4.0.0 surface, TZDB 2026c. Every warm benchmark runs against a service whose default-language catalog is already built, so the numbers are the per-call cost of "compute the temporal part": a `GetZoneInterval` binary search per zone plus the returned records. The full list at ~46 µs / 56 KB is 596 `TimeZone` records and the array holding them — nothing else; a single lookup allocates only its record (88 B) and the miss path allocates nothing. `ByWindowsIdTerritory` pays TimeZoneConverter's territory resolution (~270 ns over the default-territory path); `ByCoordinate` is dominated by GeoTimeZone. The conversions' 64 B is the boxed `ZoneLocalMapping` NodaTime hands to the resolver; `InTimeZone` needs none.

The cold start is the one deliberately expensive path — TimeZoneNames for 596 zones — measured with a fresh service per iteration under the Monitoring strategy, so it includes JIT on the first iteration and is bimodal (min 12.9 ms, median 33.2 ms, one 370 ms outlier); the median is the number to read.

###### Lists

| Method                  |       Mean |      Error |     StdDev | Allocated |
| ----------------------- | ---------: | ---------: | ---------: | --------: |
| GetTimeZones            |  47.247 us | 229.332 ns | 203.297 ns |   55.9 KB |
| GetTimeZonesAt          |  45.585 us | 236.286 ns | 197.310 ns |   55.9 KB |
| GetTimeZonesByWindowsId | 583.279 ns |   3.159 ns |   2.638 ns |    1.8 KB |

###### Lookups

| Method               |       Mean |    Error |   StdDev | Allocated |
| -------------------- | ---------: | -------: | -------: | --------: |
| ByIanaId             |  67.219 ns | 0.280 ns | 0.234 ns |      88 B |
| ByIanaIdAlias        |  78.072 ns | 0.312 ns | 0.292 ns |      88 B |
| ByIanaIdCased        |  68.011 ns | 0.116 ns | 0.097 ns |      88 B |
| ByIanaIdUnknown      |  29.234 ns | 0.034 ns | 0.030 ns |         - |
| ByWindowsId          | 127.254 ns | 0.360 ns | 0.336 ns |     160 B |
| ByWindowsIdTerritory | 398.461 ns | 1.301 ns | 1.087 ns |     232 B |
| ByCoordinate         | 237.702 ns | 1.676 ns | 1.568 ns |     272 B |

###### Conversions

| Method                   |      Mean |    Error |   StdDev | Allocated |
| ------------------------ | --------: | -------: | -------: | --------: |
| AtTimeZoneDateTime       | 85.010 ns | 0.189 ns | 0.168 ns |      64 B |
| AtTimeZoneDateTimeOffset | 82.631 ns | 1.596 ns | 1.415 ns |      64 B |
| TryAtTimeZoneAmbiguous   | 91.006 ns | 0.173 ns | 0.154 ns |      64 B |
| InTimeZone               | 53.300 ns | 0.097 ns | 0.086 ns |         - |
| AtTimeZoneTimeSpan       |  1.273 ns | 0.002 ns | 0.001 ns |         - |

###### Cold start

| Method            |     Mean |    Error |   StdDev |   Median | Allocated |
| ----------------- | -------: | -------: | -------: | -------: | --------: |
| FirstGetTimeZones | 49.71 ms | 67.76 ms | 78.03 ms | 33.16 ms |   1.54 MB |

###### 3.0.1 reference

Not BenchmarkDotNet numbers. Approximate `Stopwatch` measurements of the 3.0.1 implementation taken during the 4.0 audit, on the same machine, for orientation only: the full list ≈ 43 ms on the first call (registry lookups through `TimeZoneInfo.FindSystemTimeZoneById` for every zone) and ≈ 0.07 ms when served from the per-hour `IMemoryCache`; `GetTimeZoneByIanaId` ≈ 100 µs (a TimeZoneNames lookup per call); `GetTimeZoneByCoordinate` ≈ 130 µs.
