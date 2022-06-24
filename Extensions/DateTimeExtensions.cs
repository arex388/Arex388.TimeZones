namespace System;

public static class DateTimeExtensions {
    public static DateTime? AtTimeZone(
        this DateTime? value,
        Arex388.TimeZones.TimeZone timeZone) => value?.AtTimeZone(timeZone);

    public static DateTime AtTimeZone(
        this DateTime value,
        Arex388.TimeZones.TimeZone timeZone) => new DateTimeOffset(value).AtTimeZone(timeZone).DateTime;
}