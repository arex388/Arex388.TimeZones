namespace System;

[Obsolete("This will be removed in 2.1.0.")]
public static class DateTimeExtensions {
	[Obsolete("Use DateTimeOffset instead of DateTime. This will be removed in 2.1.0.")]
	public static DateTime? AtTimeZone(
		this DateTime? value,
		Arex388.TimeZones.TimeZone timeZone) => value?.AtTimeZone(timeZone);

	[Obsolete("Use DateTimeOffset instead of DateTime. This will be removed in 2.1.0.")]
	public static DateTime AtTimeZone(
		this DateTime value,
		Arex388.TimeZones.TimeZone timeZone) => new DateTimeOffset(value).AtTimeZone(timeZone).DateTime;
}