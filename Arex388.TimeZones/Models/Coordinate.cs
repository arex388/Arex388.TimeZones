namespace Arex388.TimeZones;

/// <summary>
/// A coordinate.
/// </summary>
public sealed class Coordinate :
    ICoordinate {
    /// <summary>
    /// The coordinate's latitude.
    /// </summary>
    public required double Latitude { get; init; }

    /// <summary>
    /// The coordinate's longitude.
    /// </summary>
    public required double Longitude { get; init; }
}