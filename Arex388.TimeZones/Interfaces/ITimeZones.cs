using NodaTime;

namespace Arex388.TimeZones;

/// <summary>
/// TimeZones service.
/// </summary>
public interface ITimeZones {
    /// <summary>
    /// Returns all time zones for the current instant in time.
    /// </summary>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    IEnumerable<TimeZone> GetTimeZones(
        string languageCode = "en-US");

    /// <summary>
    /// Returns all time zones for an instant in time.
    /// </summary>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    IEnumerable<TimeZone> GetTimeZones(
        DateTimeOffset? dateTime,
        string languageCode = "en-US");

    /// <summary>
    /// Returns all time zones for an instant in time.
    /// </summary>
    /// <param name="instant">The instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    IEnumerable<TimeZone> GetTimeZones(
        Instant instant,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByCoordinate(
        decimal latitude,
        decimal longitude);

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByCoordinate(
        double latitude,
        double longitude);

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByCoordinate(
        ICoordinate coordinate,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for the current instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByCoordinate(
        decimal latitude,
        decimal longitude,
        DateTimeOffset? dateTime,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zone by the specified latitude and longitude coordinate for an instant in time.
    /// </summary>
    /// <param name="latitude">The coordinate's latitude.</param>
    /// <param name="longitude">The coordinate's longitude.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByCoordinate(
        double latitude,
        double longitude,
        DateTimeOffset? dateTime,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zone by the specified point coordinate for an instant in time.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByCoordinate(
        ICoordinate coordinate,
        DateTimeOffset? dateTime,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zone by the IANA id for the current instant in time.
    /// </summary>
    /// <param name="ianaId">The time zone's IANA id.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByIanaId(
        string ianaId,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zone by the IANA id for an instant in time.
    /// </summary>
    /// <param name="ianaId">The time zone's IANA id.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zone.</returns>
    TimeZone? GetTimeZoneByIanaId(
        string ianaId,
        DateTimeOffset? dateTime,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zones by the Windows id for the current instant in time.
    /// </summary>
    /// <param name="windowsId">The time zones' Windows id.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    IEnumerable<TimeZone> GetTimeZonesByWindowsId(
        string windowsId,
        string languageCode = "en-US");

    /// <summary>
    /// Returns the time zones by the Windows id for the current instant in time.
    /// </summary>
    /// <param name="windowsId">The time zones' Windows id.</param>
    /// <param name="dateTime">The DateTimeOffset value to use for an instant in time.</param>
    /// <param name="languageCode">The language code to use. "en-US" by default.</param>
    /// <returns>The time zones.</returns>
    IEnumerable<TimeZone> GetTimeZonesByWindowsId(
        string windowsId,
        DateTimeOffset? dateTime,
        string languageCode = "en-US");
}