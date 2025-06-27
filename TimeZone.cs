namespace Arex388.TimeZones;

/// <summary>
/// TimeZone object.
/// </summary>
public sealed class TimeZone {
    /// <summary>
    /// The time zone's abbreviation.
    /// </summary>
    public required string Abbreviation { get; init; }

    /// <summary>
    /// The time zone's IANA id.
    /// </summary>
    public required string IanaId { get; init; }

    /// <summary>
    /// Flag indicating the time zone is currently in daylight savings.
    /// </summary>
    public required bool IsDaylightSavings { get; init; }

    /// <summary>
    /// The time zone's UTC offset.
    /// </summary>
    [Obsolete("Use UtcOffsetTs instead. This will be removed in 3.1.0.")]
    public required string UtcOffset { get; init; }

    /// <summary>
    /// The time zone's UTC offset.
    /// </summary>
    [Obsolete("This will be renamed to UtcOffset in 3.2.0.")]
    public required TimeSpan UtcOffsetTs { get; init; }

    /// <summary>
    /// The time zone's Windows id.
    /// </summary>
    public required string WindowsId { get; init; }
}