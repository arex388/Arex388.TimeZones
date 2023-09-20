namespace System;

/// <summary>
/// DateTimeOffset extensions.
/// </summary>
public static class DateTimeOffsetExtensions {
	[Obsolete("Use AtTimeZone() with UTC offset TimeSpan argument. This will be removed in 2.1.0.")]
	public static DateTimeOffset? AtTimeZone(
		this DateTimeOffset? value,
		Arex388.TimeZones.TimeZone timeZone) => value?.AtTimeZone(timeZone);

	[Obsolete("Use AtTimeZone() with UTC offset TimeSpan argument. This will be removed in 2.1.0.")]
	public static DateTimeOffset AtTimeZone(
		this DateTimeOffset value,
		Arex388.TimeZones.TimeZone timeZone) {
		var utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
		var timeSpan = TimeSpan.Parse(timeZone.UtcOffset.Replace("+", null));
		var minutes = (utcOffset - timeSpan).TotalMinutes;

		return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(value.AddMinutes(minutes), timeZone.WindowsId);
	}

	/// <summary>
	/// Returns a new DateTimeOffset value for the same date and time but with the specified UTC offset.
	/// </summary>
	/// <param name="value">The DateTimeOffset source value.</param>
	/// <param name="utcOffset">The UTC offset for the new value.</param>
	/// <returns>The new DateTimeOffset value.</returns>
	public static DateTimeOffset? AtTimeZone(
		this DateTimeOffset? value,
		TimeSpan utcOffset) => value?.AtTimeZone(utcOffset);

	/// <summary>
	/// Returns a new DateTimeOffset value for the same date and time but with the specified UTC offset.
	/// </summary>
	/// <param name="value">The DateTimeOffset source value.</param>
	/// <param name="utcOffset">The UTC offset for the new value.</param>
	/// <returns>The new DateTimeOffset value.</returns>
	public static DateTimeOffset AtTimeZone(
		this DateTimeOffset value,
		TimeSpan utcOffset) => new(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, utcOffset);
}