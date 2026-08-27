# CHANGE LOG

Dates are the NuGet publish dates (local time), reconciled against the commit history. Versions marked *(published without a commit)* exist on NuGet but have no commit of their own — their changes surface in the next committed version.

#### 3.0.1 (2025-10-18)

- **Updated:** TimeZoneConverter 7.0.0 → 7.1.0.

#### 3.0.0 (2025-06-27)

> [!WARNING]
>
> This version has breaking changes.

- **Changed:** `TimeZones` is now an `internal sealed` class implementing the new `ITimeZones` interface, instead of a public static class. Consumers resolve `ITimeZones` through dependency injection.
- **Changed:** `Microsoft.Extensions.Caching.Abstractions` is now **required** for internal use of `IMemoryCache`. The library does not register `IMemoryCache` — call `AddMemoryCache()` yourself.
- **Changed:** `Microsoft.Extensions.DependencyInjection.Abstractions` is now **required** for dependency injection registration.
- **Added:** `ITimeZones` interface for public use.
- **Added:** `AddTimeZones()` extension method on `IServiceCollection`, registering `ITimeZones` as a singleton.
- **Added:** Caching of the `GetTimeZones()` result per language code and hour, with a 1 hour absolute / 15 minute sliding expiration.

#### 2.0.8 (2025-06-27)

Previously logged as "Minor clean up and optimizations to improve performance." The commits behind it:

- **Changed:** `TimeZone` properties are now `required` with `init` setters (previously settable, with `null!` defaults). Constructing a `TimeZone` yourself now requires every property.
- **Added:** Latitude (−90 to 90) and longitude (−180 to 180) validation on the coordinate lookups; out-of-range values throw `ArgumentOutOfRangeException`.
- **Changed:** `GetTimeZoneByIanaId()` resolves the zone directly from the TZDB provider instead of scanning the full time zone list.
- **Changed:** The removal targets on the obsolete members moved from 2.1.0 / 2.2.0 to 3.1.0 / 3.2.0.

#### 2.0.7 (2025-06-27)

- **Updated:** NodaTime 3.2.1 → 3.2.2.

#### 2.0.6 (2025-02-18)

- **Updated:** GeoTimeZone 5.3.0 → 6.0.0, NodaTime 3.2.0 → 3.2.1, PolySharp 1.14.1 → 1.15.0, TimeZoneConverter 6.1.0 → 7.0.0, TimeZoneNames 6.0.0 → 7.0.0.

#### 2.0.5 (2024-10-13)

- **Updated:** NodaTime 3.1.12 → 3.2.0.

#### 2.0.4 (2024-09-13)

- **Updated:** NodaTime 3.1.9 → 3.1.12, PolySharp 1.13.2 → 1.14.1.

The commit for this version also carries the 2.0.1 and 2.0.3 changes below, which were published without commits of their own.

#### 2.0.3 (2024-02-14) *(published without a commit)*

- **Added:** New `TimeZones.GetTimeZoneByCoordinate()` methods accepting an instance of `ICoordinate`.
- **Added:** `ICoordinate` interface and `Coordinate` implementation.

#### 2.0.2 (2024-02-12) *(published without a commit)*

- **Updated:** NuGet packages.

#### 2.0.1 (2023-10-09) *(published without a commit)*

- **Changed:** Single time zones returned by a coordinate or IANA id lookup may not exist, so the return is explicitly a nullable `TimeZone?` and the selection LINQ changed from `First` to `FirstOrDefault`.

#### 2.0.0 (2023-09-20)

- **Added:** New `DateTimeOffsetExtensions.AtTimeZone()` methods accepting a `TimeSpan` UTC offset.
- **Added:** XML docs.
- **Added:** `TimeZone.UtcOffsetTs` `TimeSpan` property.
- **Added:** New `TimeZones.GetTimeZones()` overloads accepting a `DateTimeOffset` or a NodaTime `Instant`, and new `GetTimeZoneByCoordinate()`, `GetTimeZoneByIanaId()`, and `GetTimeZonesByWindowsId()` overloads accepting a `DateTimeOffset` value, to get the time zone in the past, present, or future.
- **Overhauled:** `TimeZones.GetTimeZones()`: time zones without a Windows id mapping are now excluded from the list (previously returned with a null `WindowsId`), `IsDaylightSavings` is evaluated at the requested instant rather than `DateTime.Now`, and `Abbreviation` falls back to the IANA id when no localized abbreviation exists.
- **Changed:** `GetTimeZoneByIanaId()` used `First()` and threw when a zone was not found — corrected in 2.0.1.
- **Changed:** Nullable reference types and implicit usings enabled; PolySharp 1.13.2 added for the compiler polyfills.
- **Deprecated:** Old `DateTimeOffsetExtensions.AtTimeZone()` methods accepting a `TimeZone` object.
- **Deprecated:** `DateTimeExtensions` because it's kind of irrelevant. If you want to adjust time zones of the value, just use a `DateTimeOffset` instead.
- **Deprecated:** `TimeZone.UtcOffset` property.
- **Updated:** GeoTimeZone 5.2.0 → 5.3.0.
- **Added:** MIT `LICENSE` file.

#### 1.0.24 (2023-09-17) *(published without a commit)*

- **Updated:** NuGet packages.

#### 1.0.23 (2023-03-31)

- **Updated:** NodaTime 3.1.6 → 3.1.9, TimeZoneConverter 6.0.1 → 6.1.0.

#### 1.0.22 (2023-03-24) *(published without a commit)*

- **Updated:** NuGet packages.

#### 1.0.21 (2023-02-14) *(published without a commit)*

- **Updated:** NuGet packages.

#### 1.0.20 (2022-12-27)

- **Updated:** GeoTimeZone 5.1.0 → 5.2.0, NodaTime 3.1.5 → 3.1.6.

#### 1.0.19 (2022-10-31)

- **Updated:** GeoTimeZone 5.0.0 → 5.1.0, NodaTime 3.1.2 → 3.1.5, TimeZoneNames 5.0.1 → 6.0.0.

#### 1.0.18 (2022-09-26) *(published without a commit)*

- **Updated:** NuGet packages.

#### 1.0.17 (2022-09-07)

- **Added:** `languageCode` parameter (default `"en-US"`) on `GetTimeZones()`, `GetTimeZoneByIanaId()`, and `GetTimeZonesByWindowsId()`; abbreviations were previously always resolved for `"en"`. Committed after the 1.0.16 bump, so this is the first listed version carrying it.
- **Updated:** GeoTimeZone 4.1.0 → 5.0.0, TimeZoneConverter 6.0.0 → 6.0.1 (plus the 1.0.16 updates below).

#### 1.0.16 (2022-09-04) *(published, then unlisted on NuGet)*

- **Updated:** NodaTime 3.1.0 → 3.1.2, TimeZoneConverter 5.0.0 → 6.0.0.

#### 1.0.15 (2022-08-16) *(published without a commit)*

- **Updated:** NuGet packages.

#### 1.0.14 (2022-08-14) *(published without a commit)*

- **Updated:** NuGet packages.

#### 1.0.13 (2022-06-23)

- **Changed:** Code cleanup — file-scoped namespaces and `LangVersion` `latest`. No functional change.

#### 1.0.12 (2022-06-23)

- **Updated:** NodaTime 3.0.9 → 3.1.0.

#### 1.0.11 (2022-03-29) *(published without a commit)*

- **Updated:** NuGet packages.

#### 1.0.10 (2022-03-07)

- **Added:** `DateTimeOffset.AtTimeZone(TimeZone)` and `DateTime.AtTimeZone(TimeZone)` extension methods (in the `System` namespace), returning the same date and time with the time zone changed, without shifting the value.
- **Changed:** The time zone list is now computed on every call instead of once per process, so offsets and daylight-savings flags reflect the current instant.
- **Changed:** `OffsetExtensions` is now `internal` (it was public).
- **Updated:** NodaTime 3.0.1 → 3.0.9, TimeZoneConverter 3.3.0 → 5.0.0, TimeZoneNames 4.1.0 → 5.0.1 — cumulative since 1.0.2, spanning the uncommitted 1.0.3 – 1.0.9.

#### 1.0.3 – 1.0.9 *(published without commits)*

- **Updated:** NuGet packages — 1.0.3 (2021-03-15), 1.0.4 (2021-03-21), 1.0.5 (2021-07-16), 1.0.6 (2021-09-29), 1.0.7 (2021-10-02), 1.0.8 (2021-10-22), 1.0.9 (2022-01-12). The cumulative package changes are listed under 1.0.10.

#### 1.0.2 (2020-10-11)

- **Updated:** GeoTimeZone 4.0.0 → 4.1.0, NodaTime 3.0.0 → 3.0.1, TimeZoneConverter 3.2.0 → 3.3.0, TimeZoneNames 4.0.1 → 4.1.0.

#### 1.0.1 (2020-05-27)

- **Updated:** NodaTime 2.4.7 → 3.0.0.

#### 1.0.0 (2020-02-03)

- Initial release: static `TimeZones` class with `GetTimeZones()`, `GetTimeZoneByCoordinate()` (`decimal` or `double` latitude/longitude), `GetTimeZoneByIanaId()`, and `GetTimeZonesByWindowsId()`; the `TimeZone` model (`Abbreviation`, `IanaId`, `IsDaylightSavings`, `UtcOffset`, `WindowsId`); and a public `Offset.ToFormattedString()` extension.
- Built on GeoTimeZone 4.0.0, NodaTime 2.4.7, TimeZoneConverter 3.2.0, and TimeZoneNames 4.0.1.
