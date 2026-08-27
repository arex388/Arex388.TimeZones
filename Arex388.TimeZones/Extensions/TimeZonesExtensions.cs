using System.Diagnostics.CodeAnalysis;

namespace Arex388.TimeZones;

/// <summary>
/// Nullable-lifting conveniences over <see cref="ITimeZones"/> and the offset swap on <see cref="DateTimeOffset"/>.
/// </summary>
public static class TimeZonesExtensions {
	//  ================================================================================
	//  ITimeZones, nullable-lifting
	//  ================================================================================

	/// <summary>
	/// <see cref="ITimeZones.AtTimeZone(DateTime, string)"/> lifted over <see langword="null"/>: a <see langword="null"/> wall clock returns <see langword="null"/>.
	/// </summary>
	/// <param name="timeZones">The service.</param>
	/// <param name="wallClock">The date and time as read from a clock in the zone, or <see langword="null"/>.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <returns>The wall clock with the zone's offset, or <see langword="null"/>.</returns>
	public static DateTimeOffset? AtTimeZone(
		this ITimeZones timeZones,
		DateTime? wallClock,
		string ianaId) => wallClock is null
		? null
		: timeZones.AtTimeZone(wallClock.Value, ianaId);

	/// <summary>
	/// <see cref="ITimeZones.AtTimeZone(DateTimeOffset, string)"/> lifted over <see langword="null"/>: a <see langword="null"/> wall clock returns <see langword="null"/>.
	/// </summary>
	/// <param name="timeZones">The service.</param>
	/// <param name="wallClock">The date and time as read from a clock in the zone, or <see langword="null"/>.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <returns>The wall clock with the zone's offset, or <see langword="null"/>.</returns>
	public static DateTimeOffset? AtTimeZone(
		this ITimeZones timeZones,
		DateTimeOffset? wallClock,
		string ianaId) => wallClock is null
		? null
		: timeZones.AtTimeZone(wallClock.Value, ianaId);

	/// <summary>
	/// <see cref="ITimeZones.InTimeZone"/> lifted over <see langword="null"/>: a <see langword="null"/> value returns <see langword="null"/>.
	/// </summary>
	/// <param name="timeZones">The service.</param>
	/// <param name="value">The instant, or <see langword="null"/>.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <returns>The same instant with the zone's wall clock and offset, or <see langword="null"/>.</returns>
	public static DateTimeOffset? InTimeZone(
		this ITimeZones timeZones,
		DateTimeOffset? value,
		string ianaId) => value is null
		? null
		: timeZones.InTimeZone(value.Value, ianaId);

	//  ================================================================================
	//  DateTimeOffset, offset swap
	//  ================================================================================

	/// <summary>
	/// Returns the same wall clock — to the tick — with a different offset. This is an offset swap, not a
	/// conversion; use <see cref="ITimeZones.AtTimeZone(DateTimeOffset, string)"/> to resolve the offset from a zone.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="utcOffset">The offset for the result.</param>
	/// <returns>The wall clock of <paramref name="value"/> at <paramref name="utcOffset"/>.</returns>
	public static DateTimeOffset AtTimeZone(
		this DateTimeOffset value,
		TimeSpan utcOffset) => new(value.DateTime, utcOffset);

	/// <summary>
	/// <see cref="AtTimeZone(DateTimeOffset, TimeSpan)"/> lifted over <see langword="null"/>.
	/// </summary>
	/// <param name="value">The value, or <see langword="null"/>.</param>
	/// <param name="utcOffset">The offset for the result.</param>
	/// <returns>The wall clock of <paramref name="value"/> at <paramref name="utcOffset"/>, or <see langword="null"/>.</returns>
	[return: NotNullIfNotNull(nameof(value))]
	public static DateTimeOffset? AtTimeZone(
		this DateTimeOffset? value,
		TimeSpan utcOffset) => value?.AtTimeZone(utcOffset);
}
