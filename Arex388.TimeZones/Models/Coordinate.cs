namespace Arex388.TimeZones;

/// <summary>
/// A geospatial coordinate.
/// </summary>
/// <param name="Latitude">The latitude, −90 to 90.</param>
/// <param name="Longitude">The longitude, −180 to 180.</param>
public readonly record struct Coordinate(
	double Latitude,
	double Longitude) :
	ICoordinate;
