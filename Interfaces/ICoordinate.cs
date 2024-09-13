namespace Arex388.TimeZones;

/// <summary>
/// A coordinate.
/// </summary>
public interface ICoordinate {
	/// <summary>
	/// The coordinate's latitude.
	/// </summary>
	public double Latitude { get; }

	/// <summary>
	/// The coordinate's longitude.
	/// </summary>
	public double Longitude { get; }
}