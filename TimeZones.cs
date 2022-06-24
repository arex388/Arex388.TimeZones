using GeoTimeZone;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneConverter;
using TimeZoneNames;

namespace Arex388.TimeZones;

public static class TimeZones {
    private static IEnumerable<TimeZone> GetTimeZonesCollection() {
        var clock = SystemClock.Instance.GetCurrentInstant();

        return DateTimeZoneProviders.Tzdb.Ids.Select(
            _ => DateTimeZoneProviders.Tzdb[_]).Select(
            _ => {
                var gotWindowsTimeZoneId = TZConvert.TryIanaToWindows(_.Id, out var windowsTimeZoneId);
                var isDaylightSavings = gotWindowsTimeZoneId
                                        && TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId).IsDaylightSavingTime(DateTime.Now);
                var abbreviations = TZNames.GetAbbreviationsForTimeZone(_.Id, "en");
                var abbreviation = isDaylightSavings ? abbreviations.Daylight : abbreviations.Standard;
                var offset = _.GetUtcOffset(clock);

                return new TimeZone {
                    IanaId = _.Id,
                    WindowsId = windowsTimeZoneId,
                    UtcOffset = offset.ToFormattedString(),
                    Abbreviation = abbreviation,
                    IsDaylightSavings = isDaylightSavings
                };
            });
    }

    public static IEnumerable<TimeZone> GetTimeZones() => GetTimeZonesCollection();

    public static TimeZone GetTimeZoneByCoordinate(
        decimal latitude,
        decimal longitude) {
        var lat = Convert.ToDouble(latitude);
        var lng = Convert.ToDouble(longitude);

        return GetTimeZoneByCoordinate(lat, lng);
    }

    public static TimeZone GetTimeZoneByCoordinate(
        double latitude,
        double longitude) {
        var ianaId = TimeZoneLookup.GetTimeZone(latitude, longitude);

        return GetTimeZoneByIanaId(ianaId.Result);
    }

    public static TimeZone GetTimeZoneByIanaId(
        string ianaId) => GetTimeZonesCollection().SingleOrDefault(
        _ => _.IanaId == ianaId);

    public static IEnumerable<TimeZone> GetTimeZonesByWindowsId(
        string windowsId) => GetTimeZonesCollection().Where(
        _ => _.WindowsId == windowsId);
}