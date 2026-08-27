# Arex388.TimeZones

Offline time zone discovery for .NET Standard 2.0 — by coordinate, IANA id, or Windows id — plus the two conversions consumers actually need: `AtTimeZone` (keep the wall clock, resolve the zone's offset) and `InTimeZone` (keep the instant, show it in the zone). Everything is computed from TZDB; nothing is read from the operating system, so the answers are the same on Windows, Linux, and invariant-globalization containers. It is a free replacement for the Google Maps Time Zone API.

It is a facade over four packages, each doing one thing: [GeoTimeZone](https://github.com/mj1856/GeoTimeZone) turns a coordinate into an IANA id; [NodaTime](https://github.com/nodatime/nodatime) supplies TZDB — offsets, transitions, daylight-savings rules; [TimeZoneConverter](https://github.com/mj1856/TimeZoneConverter) maps IANA ids to Windows ids and back; [TimeZoneNames](https://github.com/mj1856/TimeZoneNames) supplies the localized names and abbreviations from CLDR.

#### Install and register

```
dotnet add package Arex388.TimeZones
```

```csharp
services.AddTimeZones();

// or, with options
services.AddTimeZones(o => {
	o.DefaultLanguageCode = "de";
});
```

Then inject `ITimeZones`. It is a singleton. A registered `TimeProvider` supplies "now" for the overloads that take no instant; without one, `TimeProvider.System` does. `AddMemoryCache()` is no longer required — the library caches only what is static (the per-language catalog of zones and names) and computes offsets per call.

#### The two operations

Every time zone problem is one of two operations, and they are not interchangeable:

- **`AtTimeZone`** keeps the **wall clock** and resolves its offset in the zone. Use it when the date and time are what the user meant and only the zone is missing — a shift that starts at 09:00 in New York.
- **`InTimeZone`** keeps the **instant** and changes the wall clock to the zone's. Use it when the value already identifies a point in time and you want to show it somewhere else.

The same input, on the day US clocks spring forward:

```csharp
var wallClock = new DateTimeOffset(2026, 3, 8, 9, 0, 0, TimeSpan.Zero);

timeZones.AtTimeZone(wallClock, "America/New_York");   // 2026-03-08 09:00 -04:00 — same clock reading, the zone's offset
timeZones.InTimeZone(wallClock, "America/New_York");   // 2026-03-08 05:00 -04:00 — same instant, the zone's clock reading
```

`AtTimeZone` is lenient: a wall clock that does not exist (the spring-forward gap) is shifted forward by the gap, and one that occurs twice (the fall-back hour) resolves to its earlier occurrence. `TryAtTimeZone` tells you which happened:

```csharp
timeZones.TryAtTimeZone(new DateTime(2026, 3, 8, 2, 30, 0), "America/New_York", out var result, out var resolution);
// result: 2026-03-08 03:30 -04:00, resolution: TimeZoneResolution.Skipped

timeZones.TryAtTimeZone(new DateTime(2026, 11, 1, 1, 30, 0), "America/New_York", out result, out resolution);
// result: 2026-11-01 01:30 -04:00, resolution: TimeZoneResolution.Ambiguous
```

Both accept a `DateTime` (its `Kind` is ignored) or a `DateTimeOffset` (its offset is ignored — only the clock reading matters). Nullable-lifting overloads (`DateTime?`, `DateTimeOffset?` → `null` in, `null` out) live in `TimeZonesExtensions`, along with `DateTimeOffset.AtTimeZone(TimeSpan)`, which swaps the offset and nothing else. All return `null` for an unknown id.

#### Lookups

```csharp
timeZones.GetTimeZoneByCoordinate(40.7128, -74.0060);                  // America/New_York, as of now
timeZones.GetTimeZoneByCoordinate(new Coordinate(40.7128, -74.0060), at);
timeZones.GetTimeZoneByIanaId("America/New_York", at);                 // ids match case-insensitively; aliases such as US/Eastern work
timeZones.GetTimeZoneByWindowsId("Eastern Standard Time");             // the canonical zone — America/New_York
timeZones.GetTimeZoneByWindowsId("Eastern Standard Time", "CA");       // for a territory — America/Toronto
timeZones.GetTimeZonesByWindowsId("Eastern Standard Time");            // every zone that maps to it
timeZones.GetTimeZones(at);                                            // every zone with a Windows mapping, ordered by IANA id
```

Every lookup has an as-of-now form and an `at` form taking a `DateTimeOffset` instant (its offset is irrelevant beyond identifying the instant), and takes an optional language code for the names. Single lookups return `null` when the id is unknown or the zone has no Windows mapping; open ocean resolves to the nautical zone (`Etc/GMT±N`; `UTC` within 7.5° of the prime meridian). Argument errors throw: a `null` id, a coordinate outside ±90 / ±180 (NaN and infinity included), or a language code TimeZoneNames does not know.

`TimeZone` is a record:

- `IanaId` — as looked up, in TZDB's casing; an alias is preserved.
- `CanonicalIanaId` / `IsAlias` — `US/Eastern` → `America/New_York`.
- `WindowsId` — never `null`; zones without a mapping are excluded.
- `Name` — localized and daylight-savings-aware, `Eastern Daylight Time`; never empty. Use it for display.
- `Abbreviation` — the CLDR abbreviation for the current daylight-savings state, `EDT`; `null` when CLDR has none, never an IANA id.
- `UtcOffset` — a `TimeSpan`.
- `IsDaylightSavings` — from TZDB, normalized so that zones modelled with negative savings (Europe/Dublin, Africa/Casablanca) report summer as daylight savings.
- `At` — the instant the snapshot was computed for, in UTC.

#### Data sources and currency

- **TZDB** comes from NodaTime; `ITimeZones.TzdbVersion` reports the version in use. To pick up a tzdata release before a package update, download the matching `.nzd` from [nodatime.org/tzdb](https://nodatime.org/tzdb/) and set `TimeZonesOptions.DateTimeZoneProvider` to a `DateTimeZoneCache` over `TzdbDateTimeZoneSource.FromStream(...)`.
- **Windows mappings and localized names** come from CLDR via TimeZoneConverter and TimeZoneNames. A zone with no Windows mapping (`Antarctica/Troll` is the only one today) is excluded from every result.
- SQL Server's `AT TIME ZONE` and .NET's `TimeZoneInfo` read the Windows registry, which is updated by Windows Update on its own schedule, so they can disagree with TZDB briefly after a rule change. This library never reads the registry.

#### Sample output

The first ten rows of `GetTimeZones()` at `2026-07-15T12:00:00Z`, with the default language, from `TZDB: 2026c (mapping: 48.2)`:

| IANA Id | Windows Id | Name | Abbreviation | Offset | DST |
|---|---|---|---|---|---|
| Africa/Abidjan | Greenwich Standard Time | Greenwich Mean Time | GMT | +00:00 | False |
| Africa/Accra | Greenwich Standard Time | Greenwich Mean Time | GMT | +00:00 | False |
| Africa/Addis_Ababa | E. Africa Standard Time | East Africa Time | EAT | +03:00 | False |
| Africa/Algiers | W. Central Africa Standard Time | Central European Standard Time | CET | +01:00 | False |
| Africa/Asmara | E. Africa Standard Time | East Africa Time | EAT | +03:00 | False |
| Africa/Asmera | E. Africa Standard Time | East Africa Time | EAT | +03:00 | False |
| Africa/Bamako | Greenwich Standard Time | Greenwich Mean Time | GMT | +00:00 | False |
| Africa/Bangui | W. Central Africa Standard Time | West Africa Standard Time | WAT | +01:00 | False |
| Africa/Banjul | Greenwich Standard Time | Greenwich Mean Time | GMT | +00:00 | False |
| Africa/Bissau | Greenwich Standard Time | Greenwich Mean Time | GMT | +00:00 | False |

#### Migrating from 3.x

4.0.0 is a breaking release. Every consumer changes; the table maps each 3.0.1 call to its 4.0.0 equivalent.

| 3.0.1 | 4.0.0 |
|---|---|
| `value.AtTimeZone(timeZone)` (obsolete) | `timeZones.AtTimeZone(value, ianaId)` — and the result is now correct across DST |
| `GetTimeZoneByIanaId(id, at)` + `value.AtTimeZone(tz.UtcOffsetTs)` | `timeZones.AtTimeZone(value, id)` |
| `tz.UtcOffsetTs` | `tz.UtcOffset` (`TimeSpan`) |
| `tz.UtcOffset` (string) | `tz.UtcOffset.ToString(@"hh\:mm")` with a sign, or NodaTime `OffsetPattern` |
| `GetTimeZoneByCoordinate(decimal, decimal)` | cast to `double` |
| `GetTimeZones(Instant)` | `GetTimeZones(instant.ToDateTimeOffset())` |
| `DateTimeOffset?` at-time argument | `.Value` (the parameter is non-nullable) |
| `services.AddMemoryCache().AddTimeZones()` | `services.AddTimeZones()` — `AddMemoryCache()` no longer required (harmless) |
| `DateTime.AtTimeZone(...)` | removed; use `DateTimeOffset` or `timeZones.AtTimeZone(dateTime, ianaId)` |
| `IEnumerable<TimeZone>` results | `IReadOnlyList<TimeZone>` |
| `Abbreviation` always a string | `string?` — `null` when CLDR has no abbreviation; use `Name` for display |

Also: the public extension methods moved from `namespace System` to `Arex388.TimeZones.TimeZonesExtensions`, so a `using Arex388.TimeZones;` is needed where `AtTimeZone(TimeSpan)` is called, and a consumer's own `System.DateTimeOffsetExtensions` no longer collides with the library's.

#### License

MIT — see [LICENSE](LICENSE).
