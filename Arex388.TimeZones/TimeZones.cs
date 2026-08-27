using GeoTimeZone;
using Microsoft.Extensions.Caching.Memory;
using NodaTime;
using NodaTime.Extensions;
using TimeZoneConverter;
using TimeZoneNames;

namespace Arex388.TimeZones;

internal sealed class TimeZones(
    IMemoryCache cache) :
    ITimeZones {
    private static readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
        SlidingExpiration = TimeSpan.FromMinutes(15)
    };

    private readonly IMemoryCache _cache = cache;

    public IEnumerable<TimeZone> GetTimeZones(
        string languageCode = "en-US") {
        var instant = SystemClock.Instance.GetCurrentInstant();

        return GetTimeZones(instant, languageCode);
    }

    public IEnumerable<TimeZone> GetTimeZones(
        DateTimeOffset? dateTime,
        string languageCode = "en-US") {
        if (dateTime is null) {
            throw new ArgumentNullException(nameof(dateTime));
        }

        var instant = dateTime.Value.ToInstant();

        return GetTimeZones(instant, languageCode);
    }

    public IEnumerable<TimeZone> GetTimeZones(
        Instant instant,
        string languageCode = "en-US") {
        var hourInstant = instant.ToDateTimeOffset().Date.AddHours(instant.ToDateTimeOffset().Hour);
        var cacheKey = $"{nameof(Arex388)}.{nameof(TimeZones)}.{languageCode}.{hourInstant:yyyy-MM-dd-HH}";

        if (_cache.TryGetValue(cacheKey, out var cached)
            && cached is List<TimeZone> timeZones) {
            return timeZones;
        }

        timeZones = GetTimeZonesInternal(instant, languageCode).ToList();

        _cache.Set(cacheKey, timeZones, _cacheEntryOptions);

        return timeZones;
    }

    private static IEnumerable<TimeZone> GetTimeZonesInternal(
        Instant instant,
        string languageCode) => DateTimeZoneProviders.Tzdb.Ids.Select(
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

    public TimeZone? GetTimeZoneByCoordinate(
        decimal latitude,
        decimal longitude) => GetTimeZoneByCoordinate((double)latitude, (double)longitude);

    public TimeZone? GetTimeZoneByCoordinate(
        double latitude,
        double longitude) => GetTimeZoneByCoordinate(new Coordinate {
            Latitude = latitude,
            Longitude = longitude
        });

    public TimeZone? GetTimeZoneByCoordinate(
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

    public TimeZone? GetTimeZoneByCoordinate(
        decimal latitude,
        decimal longitude,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") => GetTimeZoneByCoordinate((double)latitude, (double)longitude, dateTime, languageCode);

    public TimeZone? GetTimeZoneByCoordinate(
        double latitude,
        double longitude,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") => GetTimeZoneByCoordinate(new Coordinate {
            Latitude = latitude,
            Longitude = longitude
        }, dateTime, languageCode);

    public TimeZone? GetTimeZoneByCoordinate(
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

    public TimeZone? GetTimeZoneByIanaId(
        string ianaId,
        string languageCode = "en-US") {
        var instant = SystemClock.Instance.GetCurrentInstant();

        return GetTimeZoneByIanaId(ianaId, instant, languageCode);
    }

    public TimeZone? GetTimeZoneByIanaId(
        string ianaId,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") {
        if (dateTime is null) {
            throw new ArgumentNullException(nameof(dateTime));
        }

        var instant = dateTime.Value.ToInstant();

        return GetTimeZoneByIanaId(ianaId, instant, languageCode);
    }

    private TimeZone? GetTimeZoneByIanaId(
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

    public IEnumerable<TimeZone> GetTimeZonesByWindowsId(
        string windowsId,
        string languageCode = "en-US") => GetTimeZones(languageCode).Where(
        tz => tz?.WindowsId == windowsId);

    public IEnumerable<TimeZone> GetTimeZonesByWindowsId(
        string windowsId,
        DateTimeOffset? dateTime,
        string languageCode = "en-US") => GetTimeZones(dateTime, languageCode).Where(
        tz => tz?.WindowsId == windowsId);
}