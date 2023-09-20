namespace Arex388.TimeZones;

/// <summary>
/// TimeZone object.
/// </summary>
public sealed class TimeZone {
	/// <summary>
	/// The time zone's abbreviation.
	/// </summary>
	public string Abbreviation { get; set; } = null!;

	/// <summary>
	/// The time zone's IANA id.
	/// </summary>
	public string IanaId { get; set; } = null!;

	/// <summary>
	/// Flag indicating the time zone is currently in daylight savings.
	/// </summary>
	public bool IsDaylightSavings { get; set; }

	/// <summary>
	/// The time zone's UTC offset.
	/// </summary>
	[Obsolete("Use UtcOffsetTs instead. This will be removed in 2.1.0.")]
	public string UtcOffset { get; set; } = null!;

	/// <summary>
	/// The time zone's UTC offset.
	/// </summary>
	[Obsolete("This will be renamed to UtcOffset in 2.2.0.")]
	public TimeSpan UtcOffsetTs { get; set; }

	/// <summary>
	/// The time zone's Windows id.
	/// </summary>
	public string WindowsId { get; set; } = null!;
}