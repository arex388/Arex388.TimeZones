﻿#### 2.0.6 (2025-02-18)

- **Updated:** NuGet packages.

#### 2.0.5 (2024-10-13)

- **Updated:** NuGet packages.

#### 2.0.4

- **Updated:** NuGet packages.

#### 2.0.3

- **Added:** New `TimeZones.GetTimeZoneByCoordinate()` methods accepting an instance of `ICoordinate`.
- **Added:** `ICoordinate` interface and `Coordinate` implementation.

#### 2.0.2

- **Updated:** NuGet packages.

#### 2.0.1

- **Changed:** Single time zones returned by a coordinate or IANA id lookup may not exist, so the return is explicitly a nullable `TimeZone` and the selection LINQ has been changed to `FirstOrDefault`.

#### 2.0.0

- **Added:** New `DateTimeOffsetExtensions.AtTimeZone()` methods accepting a `TimeSpan` UTC offset.
- **Added:** XML docs.
- **Added:** `TimeZone.UtcOffsetTs` `TimeSpan` property.
- **Added:** New `TimeZones.GetTimeZoneByCoordinate()` method accepting a `DateTimeOffset` value to get the time zone in the past, present, or future.
- **Added:** New `TimeZones.GetTimeZoneByIanaId()` method accepting a `DateTimeOffset` value to get the time zone in the past, present, or future.
- **Added:** New `TimeZones.GetTimeZoneByWindowsId()` method accepting a `DateTimeOffset` value to get the time zone in the past, present, or future.
- **Overhauled:** `TimeZones.GetTimeZones()` method.
- **Deprecated:** Old `DateTimeOffsetExtensions.AtTimeZone()` methods accepting a `TimeZone` object.
- **Deprecated:** `DateTimeExtensions` because it's kind of irrelevant. If you want to adjust time zones of the value, just use a `DateTimeOffset` instead.
- **Deprecated:** `TimeZone.UtcOffset` property.

#### 1.0.0 - 1.0.24

- Initial release and subsequent dependency updates.