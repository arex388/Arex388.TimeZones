using GeoTimeZone;
using NodaTime;
using NodaTime.Extensions;
using TimeZoneConverter;
using TimeZoneNames;

namespace Arex388.TimeZones;

/// <summary>
/// TimeZones object.
/// </summary>
public static class TimeZones {
    /// <summary>
    /// Returns all time zones for the current instant in time.
    /// </summary>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    public static IEnumerable<TimeZone> GetTimeZones(
        string languageCode = "en-US") {
        var instant = SystemClock.Instance.GetCurrentInstant();

        return GetTimeZones(instant, languageCode);
    }

    /// <summary>
    /// Returns all time zones for an instant in time.
    /// </summary>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    public static IEnumerable<TimeZone> GetTimeZones(
        DateTimeOffset? dateTime,
        string languageCode = "en-US") {
        if (dateTime is null) {
            throw new ArgumentNullException(nameof(dateTime));
        }

        var instant = dateTime.Value.ToInstant();

        return GetTimeZones(instant, languageCode);
    }

    /// <summary>
    /// Returns all time zones for an instant in time.
    /// </summary>
    /// <param name="instant">The instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    public static IEnumerable<TimeZone> GetTimeZones(
        Instant instant,
        string languageCode = "en-US") => DateTimeZoneProviders.Tzdb.Ids.Select(
        id => DateTimeZoneProviders.Tzdb[id]).Select(
        dtz => {
            var gotWindowsTimeZoneId = TZConvert.TryIanaToWindows(dtz.Id, out var windowsTimeZoneId);

            if (!gotWindowsTimeZoneId
                || windowsTimeZoneId is null) {
                return null;
            }

            var isDaylightSavings = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId).IsDaylightSavingTime(instant.ToDateTimeOffset());
            var abbreviation = TZNames.GetAbbreviationsForTimeZone(dtz.Id, languageCode);
            var offset = dtz.GetUtcOffset(instant);

            return new TimeZone {
                Abbreviation = (isDaylightSavings
                    ? abbreviation.Daylight
                    : abbreviation.Standard) ?? dtz.Id,
                IanaId = dtz.Id,
                IsDaylightSavings = isDaylightSavings,
                UtcOffset = offset.ToFormattedString(),
                UtcOffsetTs = offset.ToTimeSpan(),
                WindowsId = windowsTimeZoneId
            };
        }).Where(
        tz => tz is not null);

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByCoordinate(
        decimal latitude,
        decimal longitude) => GetTimeZoneByCoordinate((double)latitude, (double)longitude);

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByCoordinate(
        double latitude,
        double longitude) => GetTimeZoneByCoordinate(new Coordinate {
            Latitude = latitude,
            Longitude = longitude
        });

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByCoordinate(
        ICoordinate coordinate,
        string languageCode = "en-US") {
        if (coordinate.Latitude is < -90 or > 90) {
            throw new ArgumentOutOfRangeException(nameof(coordinate.Latitude), $"Latitude must be between -90 and 90. Received: {coordinate.Latitude}");
        }

        if (coordinate.Longitude is < -180 or > 180) {
            throw new ArgumentOutOfRangeException(nameof(coordinate.Longitude), $"Longitude must be between -180 and 180. Received: {coordinate.Longitude}");
        }

        var ianaId = TimeZoneLookup.GetTimeZone(coordinate.Latitude, coordinate.Longitude);

        return GetTimeZoneByIanaId(ianaId.Result, languageCode);
    }

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByCoordinate(
        decimal latitude,
        decimal longitude,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") => GetTimeZoneByCoordinate((double)latitude, (double)longitude, dateTime, languageCode);

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for an instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByCoordinate(
        double latitude,
        double longitude,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") => GetTimeZoneByCoordinate(new Coordinate {
            Latitude = latitude,
            Longitude = longitude
        }, dateTime, languageCode);

    /// <summary>
    /// Returns the time zone by the specified point coordinate for an instant in time.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByCoordinate(
        ICoordinate coordinate,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") {
        if (coordinate.Latitude is < -90 or > 90) {
            throw new ArgumentOutOfRangeException(nameof(coordinate.Latitude), $"Latitude must be between -90 and 90. Received: {coordinate.Latitude}");
        }

        if (coordinate.Longitude is < -180 or > 180) {
            throw new ArgumentOutOfRangeException(nameof(coordinate.Longitude), $"Longitude must be between -180 and 180. Received: {coordinate.Longitude}");
        }

        var ianaId = TimeZoneLookup.GetTimeZone(coordinate.Latitude, coordinate.Longitude);

        return GetTimeZoneByIanaId(ianaId.Result, dateTime, languageCode);
    }

    /// <summary>
    /// Returns the time zone by the IANA id for the current instant in time.
    /// </summary>
    /// <param name="ianaId">The time zone's IANA id.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByIanaId(
        string ianaId,
        string languageCode = "en-US") {
        var instant = SystemClock.Instance.GetCurrentInstant();

        return GetTimeZoneByIanaId(ianaId, instant, languageCode);
    }

    /// <summary>
    /// Returns the time zone by the IANA id for an instant in time.
    /// </summary>
    /// <param name="ianaId">The time zone's IANA id.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    public static TimeZone? GetTimeZoneByIanaId(
        string ianaId,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") {
        if (dateTime is null) {
            throw new ArgumentNullException(nameof(dateTime));
        }

        var instant = dateTime.Value.ToInstant();

        return GetTimeZoneByIanaId(ianaId, instant, languageCode);
    }

    /// <summary>
    /// Returns the time zone by the IANA id for an instant in time.
    /// </summary>
    /// <param name="ianaId">The time zone's IANA id.</param>
    /// <param name="instant">The instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    private static TimeZone? GetTimeZoneByIanaId(
        string ianaId,
        Instant instant,
        string languageCode = "en-US") {
        var dtz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(ianaId);

        if (dtz is null) {
            return null;
        }

        var gotWindowsTimeZoneId = TZConvert.TryIanaToWindows(dtz.Id, out var windowsTimeZoneId);

        if (!gotWindowsTimeZoneId
            || windowsTimeZoneId is null) {
            return null;
        }

        var isDaylightSavings = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId).IsDaylightSavingTime(instant.ToDateTimeOffset());
        var abbreviation = TZNames.GetAbbreviationsForTimeZone(dtz.Id, languageCode);
        var offset = dtz.GetUtcOffset(instant);

        return new TimeZone {
            Abbreviation = (isDaylightSavings
                ? abbreviation.Daylight
                : abbreviation.Standard) ?? dtz.Id,
            IanaId = dtz.Id,
            IsDaylightSavings = isDaylightSavings,
            UtcOffset = offset.ToFormattedString(),
            UtcOffsetTs = offset.ToTimeSpan(),
            WindowsId = windowsTimeZoneId
        };
    }

    /// <summary>
    /// Returns the time zones by the Windows id for the current instant in time.
    /// </summary>
    /// <param name="windowsId">The time zones' Windows id.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    public static IEnumerable<TimeZone> GetTimeZonesByWindowsId(
        string windowsId,
        string languageCode = "en-US") => GetTimeZones(languageCode).Where(
        tz => tz?.WindowsId == windowsId);

    /// <summary>
    /// Returns the time zones by the Windows id for the current instant in time.
    /// </summary>
    /// <param name="windowsId">The time zones' Windows id.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    public static IEnumerable<TimeZone> GetTimeZonesByWindowsId(
        string windowsId,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") => GetTimeZones(dateTime, languageCode).Where(
        tz => tz?.WindowsId == windowsId);
}