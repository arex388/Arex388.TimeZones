namespace Arex388.TimeZones;

/// <summary>
/// A time zone as of one instant: its ids, localized names, offset, and daylight-savings state.
/// </summary>
public sealed record TimeZone {
	/// <summary>
	/// The IANA id as looked up, in TZDB's casing — an alias such as <c>US/Eastern</c> is preserved.
	/// </summary>
	public required string IanaId { get; init; }

	/// <summary>
	/// The canonical IANA id; equal to <see cref="IanaId"/> unless that is an alias.
	/// </summary>
	public required string CanonicalIanaId { get; init; }

	/// <summary>
	/// Whether <see cref="IanaId"/> is an alias of <see cref="CanonicalIanaId"/>.
	/// </summary>
	public bool IsAlias => !string.Equals(IanaId, CanonicalIanaId, StringComparison.Ordinal);

	/// <summary>
	/// The Windows id, for example <c>Eastern Standard Time</c>.
	/// </summary>
	public required string WindowsId { get; init; }

	/// <summary>
	/// The localized, daylight-savings-aware display name, for example <c>Eastern Daylight Time</c>. Never empty.
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// The localized CLDR abbreviation for the current daylight-savings state, for example <c>EDT</c>, or
	/// <see langword="null"/> when CLDR has none. Never empty and never an IANA id; use <see cref="Name"/> for display.
	/// </summary>
	public required string? Abbreviation { get; init; }

	/// <summary>
	/// The offset from UTC at <see cref="At"/>.
	/// </summary>
	public required TimeSpan UtcOffset { get; init; }

	/// <summary>
	/// Whether daylight savings is in effect at <see cref="At"/>, computed from TZDB. Zones with negative daylight
	/// savings (Europe/Dublin, Africa/Casablanca) report the user-facing state: summer is daylight savings. A zone
	/// on permanent time (its current TZDB interval has no end) is never in daylight savings.
	/// </summary>
	public required bool IsDaylightSavings { get; init; }

	/// <summary>
	/// The instant this snapshot was computed for, in UTC.
	/// </summary>
	public required DateTimeOffset At { get; init; }
}
