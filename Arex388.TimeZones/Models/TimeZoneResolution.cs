namespace Arex388.TimeZones;

/// <summary>
/// How a wall clock resolved in a zone — see <see cref="ITimeZones.TryAtTimeZone(DateTimeOffset, string, out DateTimeOffset, out TimeZoneResolution)"/>.
/// </summary>
public enum TimeZoneResolution {
	/// <summary>
	/// The wall clock occurs exactly once in the zone.
	/// </summary>
	Unambiguous,

	/// <summary>
	/// The wall clock occurs twice (the fall-back hour); the earlier occurrence was returned.
	/// </summary>
	Ambiguous,

	/// <summary>
	/// The wall clock does not exist (the spring-forward gap); it was shifted forward by the length of the gap.
	/// </summary>
	Skipped
}
