namespace Arex388.TimeZones;

/// <summary>
/// Time zone discovery — by coordinate, IANA id, or Windows id — and wall-clock ⇄ zone conversion,
/// computed entirely from TZDB. Nothing is read from the operating system.
/// </summary>
/// <remarks>
/// Every <c>at</c> parameter is an <em>instant</em>: its offset only identifies the point in time and has
/// no effect on the result. Every <c>languageCode</c> parameter defaults to
/// <see cref="TimeZonesOptions.DefaultLanguageCode"/> when <see langword="null"/>. Single lookups return
/// <see langword="null"/> for an unknown IANA or Windows id and for a zone without a Windows mapping;
/// argument errors throw.
/// <para>
/// Two limits of <see cref="DateTimeOffset"/> apply to the conversions. It carries offsets only to the minute,
/// so for the rare historical intervals with a sub-minute offset (local mean time before standard time; after
/// 1950 only <c>Africa/Monrovia</c> until 1972, <c>Pacific/Niue</c>, and <c>Pacific/Rarotonga</c>) the result is
/// truncated to the minute — <see cref="InTimeZone"/> is then up to 59 seconds off the instant, and
/// <see cref="TimeZone.UtcOffset"/>, which is exact, cannot be passed to
/// <see cref="TimeZonesExtensions.AtTimeZone(DateTimeOffset, TimeSpan)"/>. And a result outside the
/// <see cref="DateTimeOffset"/> range (a wall clock or instant within hours of
/// <see cref="DateTimeOffset.MinValue"/> / <see cref="DateTimeOffset.MaxValue"/>) throws
/// <see cref="ArgumentOutOfRangeException"/> or <see cref="InvalidOperationException"/> from the conversion.
/// </para>
/// </remarks>
public interface ITimeZones {
	/// <summary>
	/// The TZDB version in use, as reported by the configured NodaTime provider (for example
	/// <c>TZDB: 2026c (mapping: 48.2)</c>).
	/// </summary>
	string TzdbVersion { get; }

	//  ================================================================================
	//  Lists
	//  ================================================================================

	/// <summary>
	/// Returns every time zone with a Windows mapping, as of now, ordered by IANA id.
	/// </summary>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zones.</returns>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	IReadOnlyList<TimeZone> GetTimeZones(
		string? languageCode = null);

	/// <summary>
	/// Returns every time zone with a Windows mapping, as of an instant, ordered by IANA id.
	/// </summary>
	/// <param name="at">The instant to compute offsets and daylight-savings state for.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zones.</returns>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	IReadOnlyList<TimeZone> GetTimeZones(
		DateTimeOffset at,
		string? languageCode = null);

	//  ================================================================================
	//  By coordinate
	//  ================================================================================

	/// <summary>
	/// Returns the time zone at a coordinate, as of now. Open ocean resolves to the nautical zone (<c>Etc/GMT±N</c>; <c>UTC</c> within 7.5° of the prime meridian).
	/// </summary>
	/// <param name="latitude">The latitude, −90 to 90.</param>
	/// <param name="longitude">The longitude, −180 to 180.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the resolved zone has no Windows mapping.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="latitude"/> or <paramref name="longitude"/> is out of range, NaN, or infinite.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByCoordinate(
		double latitude,
		double longitude,
		string? languageCode = null);

	/// <summary>
	/// Returns the time zone at a coordinate, as of an instant. Open ocean resolves to the nautical zone (<c>Etc/GMT±N</c>; <c>UTC</c> within 7.5° of the prime meridian).
	/// </summary>
	/// <param name="latitude">The latitude, −90 to 90.</param>
	/// <param name="longitude">The longitude, −180 to 180.</param>
	/// <param name="at">The instant to compute the offset and daylight-savings state for.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the resolved zone has no Windows mapping.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="latitude"/> or <paramref name="longitude"/> is out of range, NaN, or infinite.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByCoordinate(
		double latitude,
		double longitude,
		DateTimeOffset at,
		string? languageCode = null);

	/// <summary>
	/// Returns the time zone at a coordinate, as of now. Open ocean resolves to the nautical zone (<c>Etc/GMT±N</c>; <c>UTC</c> within 7.5° of the prime meridian).
	/// </summary>
	/// <param name="coordinate">The coordinate.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the resolved zone has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="coordinate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The coordinate's latitude or longitude is out of range, NaN, or infinite.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByCoordinate(
		ICoordinate coordinate,
		string? languageCode = null);

	/// <summary>
	/// Returns the time zone at a coordinate, as of an instant. Open ocean resolves to the nautical zone (<c>Etc/GMT±N</c>; <c>UTC</c> within 7.5° of the prime meridian).
	/// </summary>
	/// <param name="coordinate">The coordinate.</param>
	/// <param name="at">The instant to compute the offset and daylight-savings state for.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the resolved zone has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="coordinate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">The coordinate's latitude or longitude is out of range, NaN, or infinite.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByCoordinate(
		ICoordinate coordinate,
		DateTimeOffset at,
		string? languageCode = null);

	//  ================================================================================
	//  By IANA id
	//  ================================================================================

	/// <summary>
	/// Returns the time zone for an IANA id (matched case-insensitively; aliases such as <c>US/Eastern</c> are accepted), as of now.
	/// </summary>
	/// <param name="ianaId">The IANA id.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the id is unknown or has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="ianaId"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByIanaId(
		string ianaId,
		string? languageCode = null);

	/// <summary>
	/// Returns the time zone for an IANA id (matched case-insensitively; aliases such as <c>US/Eastern</c> are accepted), as of an instant.
	/// </summary>
	/// <param name="ianaId">The IANA id.</param>
	/// <param name="at">The instant to compute the offset and daylight-savings state for.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the id is unknown or has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="ianaId"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByIanaId(
		string ianaId,
		DateTimeOffset at,
		string? languageCode = null);

	//  ================================================================================
	//  By Windows id
	//  ================================================================================

	/// <summary>
	/// Returns the canonical IANA time zone for a Windows id (matched case-insensitively), optionally for a territory, as of now.
	/// </summary>
	/// <param name="windowsId">The Windows id, for example <c>Eastern Standard Time</c>.</param>
	/// <param name="territory">An ISO 3166 territory code, for example <c>CA</c> to get <c>America/Toronto</c>; the default territory is used when <see langword="null"/> or unmapped.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the Windows id is unknown.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="windowsId"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByWindowsId(
		string windowsId,
		string? territory = null,
		string? languageCode = null);

	/// <summary>
	/// Returns the canonical IANA time zone for a Windows id (matched case-insensitively), optionally for a territory, as of an instant.
	/// </summary>
	/// <param name="windowsId">The Windows id, for example <c>Eastern Standard Time</c>.</param>
	/// <param name="at">The instant to compute the offset and daylight-savings state for.</param>
	/// <param name="territory">An ISO 3166 territory code, for example <c>CA</c> to get <c>America/Toronto</c>; the default territory is used when <see langword="null"/> or unmapped.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zone, or <see langword="null"/> if the Windows id is unknown.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="windowsId"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	TimeZone? GetTimeZoneByWindowsId(
		string windowsId,
		DateTimeOffset at,
		string? territory = null,
		string? languageCode = null);

	/// <summary>
	/// Returns every IANA time zone that maps to a Windows id (matched case-insensitively), as of now, ordered by IANA id.
	/// </summary>
	/// <param name="windowsId">The Windows id, for example <c>Eastern Standard Time</c>.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zones; empty if the Windows id is unknown.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="windowsId"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	IReadOnlyList<TimeZone> GetTimeZonesByWindowsId(
		string windowsId,
		string? languageCode = null);

	/// <summary>
	/// Returns every IANA time zone that maps to a Windows id (matched case-insensitively), as of an instant, ordered by IANA id.
	/// </summary>
	/// <param name="windowsId">The Windows id, for example <c>Eastern Standard Time</c>.</param>
	/// <param name="at">The instant to compute offsets and daylight-savings state for.</param>
	/// <param name="languageCode">The language for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/>.</param>
	/// <returns>The time zones; empty if the Windows id is unknown.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="windowsId"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="languageCode"/> is not a language TimeZoneNames knows.</exception>
	IReadOnlyList<TimeZone> GetTimeZonesByWindowsId(
		string windowsId,
		DateTimeOffset at,
		string? languageCode = null);

	//  ================================================================================
	//  Conversions
	//  ================================================================================

	/// <summary>
	/// Keeps the wall clock and resolves its offset in a zone. The input's <see cref="DateTime.Kind"/> is ignored.
	/// </summary>
	/// <remarks>
	/// Use this when the date and time are what the user meant and only the zone is missing — a shift that starts
	/// at 09:00 in <c>America/New_York</c> is <c>09:00 −05:00</c> in January and <c>09:00 −04:00</c> in July.
	/// A wall clock that does not exist (the spring-forward gap) is shifted forward by the gap; one that occurs
	/// twice (the fall-back hour) resolves to its earlier occurrence. Use
	/// <see cref="TryAtTimeZone(DateTime, string, out DateTimeOffset, out TimeZoneResolution)"/> to learn which happened.
	/// To convert an instant instead, use <see cref="InTimeZone"/>.
	/// </remarks>
	/// <param name="wallClock">The date and time as read from a clock in the zone.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <returns>The wall clock with the zone's offset, or <see langword="null"/> if the id is unknown or has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="ianaId"/> is <see langword="null"/>.</exception>
	DateTimeOffset? AtTimeZone(
		DateTime wallClock,
		string ianaId);

	/// <summary>
	/// Keeps the wall clock and resolves its offset in a zone. The input's offset is ignored.
	/// </summary>
	/// <remarks>
	/// Use this when the date and time are what the user meant and only the zone is missing — a shift that starts
	/// at 09:00 in <c>America/New_York</c> is <c>09:00 −05:00</c> in January and <c>09:00 −04:00</c> in July,
	/// whatever offset the input carried. A wall clock that does not exist (the spring-forward gap) is shifted
	/// forward by the gap; one that occurs twice (the fall-back hour) resolves to its earlier occurrence. Use
	/// <see cref="TryAtTimeZone(DateTimeOffset, string, out DateTimeOffset, out TimeZoneResolution)"/> to learn
	/// which happened. To convert an instant instead, use <see cref="InTimeZone"/>.
	/// </remarks>
	/// <param name="wallClock">The date and time as read from a clock in the zone; its offset is discarded.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <returns>The wall clock with the zone's offset, or <see langword="null"/> if the id is unknown or has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="ianaId"/> is <see langword="null"/>.</exception>
	DateTimeOffset? AtTimeZone(
		DateTimeOffset wallClock,
		string ianaId);

	/// <summary>
	/// Keeps the wall clock and resolves its offset in a zone, reporting how the resolution went.
	/// </summary>
	/// <param name="wallClock">The date and time as read from a clock in the zone; its <see cref="DateTime.Kind"/> is ignored.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <param name="result">The wall clock with the zone's offset; <see langword="default"/> when the method returns <see langword="false"/>.</param>
	/// <param name="resolution">Whether the wall clock was unambiguous, occurred twice (earlier occurrence returned), or did not exist (shifted forward by the gap).</param>
	/// <returns><see langword="true"/> if the zone was found; <see langword="false"/> if the id is unknown or has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="ianaId"/> is <see langword="null"/>.</exception>
	bool TryAtTimeZone(
		DateTime wallClock,
		string ianaId,
		out DateTimeOffset result,
		out TimeZoneResolution resolution);

	/// <summary>
	/// Keeps the wall clock and resolves its offset in a zone, reporting how the resolution went.
	/// </summary>
	/// <param name="wallClock">The date and time as read from a clock in the zone; its offset is discarded.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <param name="result">The wall clock with the zone's offset; <see langword="default"/> when the method returns <see langword="false"/>.</param>
	/// <param name="resolution">Whether the wall clock was unambiguous, occurred twice (earlier occurrence returned), or did not exist (shifted forward by the gap).</param>
	/// <returns><see langword="true"/> if the zone was found; <see langword="false"/> if the id is unknown or has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="ianaId"/> is <see langword="null"/>.</exception>
	bool TryAtTimeZone(
		DateTimeOffset wallClock,
		string ianaId,
		out DateTimeOffset result,
		out TimeZoneResolution resolution);

	/// <summary>
	/// Keeps the instant and changes the wall clock to the zone's.
	/// </summary>
	/// <remarks>
	/// Use this when the value already identifies a point in time and you want to show it somewhere else —
	/// <c>2026-07-15T12:00:00Z</c> in <c>Asia/Kolkata</c> is <c>17:30 +05:30</c>, the same instant. To keep the
	/// wall clock and change the zone instead, use <see cref="AtTimeZone(DateTimeOffset, string)"/>.
	/// </remarks>
	/// <param name="value">The instant.</param>
	/// <param name="ianaId">The IANA id of the zone.</param>
	/// <returns>The same instant with the zone's wall clock and offset, or <see langword="null"/> if the id is unknown or has no Windows mapping.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="ianaId"/> is <see langword="null"/>.</exception>
	DateTimeOffset? InTimeZone(
		DateTimeOffset value,
		string ianaId);
}
