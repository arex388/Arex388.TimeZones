using System.Collections.Concurrent;
using GeoTimeZone;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.TimeZones;
using TimeZoneConverter;
using TimeZoneNames;

namespace Arex388.TimeZones;

internal sealed class TimeZones :
	ITimeZones {
	private static readonly ZoneLocalMappingResolver _lenientResolver = Resolvers.CreateMappingResolver(Resolvers.ReturnEarlier, Resolvers.ReturnForwardShifted);

	private readonly ConcurrentDictionary<string, Lazy<TimeZoneCatalog>> _catalogs = new(StringComparer.OrdinalIgnoreCase);
	private readonly string _defaultLanguageCode;
	private readonly IDateTimeZoneProvider _provider;
	private readonly TimeProvider _timeProvider;

	public TimeZones(
		TimeZonesOptions options,
		TimeProvider timeProvider) {
		_defaultLanguageCode = options.DefaultLanguageCode ?? throw new ArgumentException($"{nameof(TimeZonesOptions.DefaultLanguageCode)} must not be null.", nameof(options));
		_provider = options.DateTimeZoneProvider ?? DateTimeZoneProviders.Tzdb;
		_timeProvider = timeProvider;
	}

	public string TzdbVersion => _provider.VersionId;

	//  ================================================================================
	//  Lists
	//  ================================================================================

	public IReadOnlyList<TimeZone> GetTimeZones(
		string? languageCode = null) => GetTimeZones(Now, languageCode);

	public IReadOnlyList<TimeZone> GetTimeZones(
		DateTimeOffset at,
		string? languageCode = null) => Snapshot(GetCatalog(languageCode).Entries, at);

	//  ================================================================================
	//  By coordinate
	//  ================================================================================

	public TimeZone? GetTimeZoneByCoordinate(
		double latitude,
		double longitude,
		string? languageCode = null) => GetTimeZoneByCoordinate(latitude, longitude, Now, languageCode);

	public TimeZone? GetTimeZoneByCoordinate(
		double latitude,
		double longitude,
		DateTimeOffset at,
		string? languageCode = null) => GetTimeZoneByCoordinateCore(latitude, longitude, nameof(latitude), nameof(longitude), at, languageCode);

	public TimeZone? GetTimeZoneByCoordinate(
		ICoordinate coordinate,
		string? languageCode = null) => GetTimeZoneByCoordinate(coordinate, Now, languageCode);

	public TimeZone? GetTimeZoneByCoordinate(
		ICoordinate coordinate,
		DateTimeOffset at,
		string? languageCode = null) {
		if (coordinate is null) {
			throw new ArgumentNullException(nameof(coordinate));
		}

		return GetTimeZoneByCoordinateCore(coordinate.Latitude, coordinate.Longitude, nameof(coordinate), nameof(coordinate), at, languageCode);
	}

	private TimeZone? GetTimeZoneByCoordinateCore(
		double latitude,
		double longitude,
		string latitudeParamName,
		string longitudeParamName,
		DateTimeOffset at,
		string? languageCode) {
		if (!(latitude is >= -90 and <= 90)) {
			throw new ArgumentOutOfRangeException(latitudeParamName, latitude, "Latitude must be between -90 and 90.");
		}

		if (!(longitude is >= -180 and <= 180)) {
			throw new ArgumentOutOfRangeException(longitudeParamName, longitude, "Longitude must be between -180 and 180.");
		}

		var catalog = GetCatalog(languageCode);
		var ianaId = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
		var entry = catalog.GetByIanaId(ianaId);

		return entry is null
			? null
			: TimeZoneCatalog.Snapshot(entry, at.ToInstant());
	}

	//  ================================================================================
	//  By IANA id
	//  ================================================================================

	public TimeZone? GetTimeZoneByIanaId(
		string ianaId,
		string? languageCode = null) => GetTimeZoneByIanaId(ianaId, Now, languageCode);

	public TimeZone? GetTimeZoneByIanaId(
		string ianaId,
		DateTimeOffset at,
		string? languageCode = null) {
		if (ianaId is null) {
			throw new ArgumentNullException(nameof(ianaId));
		}

		var entry = GetCatalog(languageCode).GetByIanaId(ianaId);

		return entry is null
			? null
			: TimeZoneCatalog.Snapshot(entry, at.ToInstant());
	}

	//  ================================================================================
	//  By Windows id
	//  ================================================================================

	public TimeZone? GetTimeZoneByWindowsId(
		string windowsId,
		string? territory = null,
		string? languageCode = null) => GetTimeZoneByWindowsId(windowsId, Now, territory, languageCode);

	public TimeZone? GetTimeZoneByWindowsId(
		string windowsId,
		DateTimeOffset at,
		string? territory = null,
		string? languageCode = null) {
		if (windowsId is null) {
			throw new ArgumentNullException(nameof(windowsId));
		}

		var catalog = GetCatalog(languageCode);

		if (!TZConvert.TryWindowsToIana(windowsId, territory ?? "001", out var ianaId)
			&& !TZConvert.TryWindowsToIana(windowsId, out ianaId)) {
			return null;
		}

		var entry = catalog.GetByIanaId(ianaId);

		return entry is null
			? null
			: TimeZoneCatalog.Snapshot(entry, at.ToInstant());
	}

	public IReadOnlyList<TimeZone> GetTimeZonesByWindowsId(
		string windowsId,
		string? languageCode = null) => GetTimeZonesByWindowsId(windowsId, Now, languageCode);

	public IReadOnlyList<TimeZone> GetTimeZonesByWindowsId(
		string windowsId,
		DateTimeOffset at,
		string? languageCode = null) {
		if (windowsId is null) {
			throw new ArgumentNullException(nameof(windowsId));
		}

		return Snapshot(GetCatalog(languageCode).GetByWindowsId(windowsId), at);
	}

	//  ================================================================================
	//  Conversions
	//  ================================================================================

	public DateTimeOffset? AtTimeZone(
		DateTime wallClock,
		string ianaId) => TryAtTimeZone(wallClock, ianaId, out var result, out _)
		? result
		: null;

	public DateTimeOffset? AtTimeZone(
		DateTimeOffset wallClock,
		string ianaId) => TryAtTimeZone(wallClock.DateTime, ianaId, out var result, out _)
		? result
		: null;

	public bool TryAtTimeZone(
		DateTimeOffset wallClock,
		string ianaId,
		out DateTimeOffset result,
		out TimeZoneResolution resolution) => TryAtTimeZone(wallClock.DateTime, ianaId, out result, out resolution);

	public bool TryAtTimeZone(
		DateTime wallClock,
		string ianaId,
		out DateTimeOffset result,
		out TimeZoneResolution resolution) {
		if (ianaId is null) {
			throw new ArgumentNullException(nameof(ianaId));
		}

		var entry = GetCatalog(null).GetByIanaId(ianaId);

		if (entry is null) {
			result = default;
			resolution = default;

			return false;
		}

		var local = LocalDateTime.FromDateTime(DateTime.SpecifyKind(wallClock, DateTimeKind.Unspecified));
		var mapping = entry.Zone.MapLocal(local);

		result = _lenientResolver(mapping).ToDateTimeOffset();
		resolution = mapping.Count switch {
			0 => TimeZoneResolution.Skipped,
			1 => TimeZoneResolution.Unambiguous,
			_ => TimeZoneResolution.Ambiguous
		};

		return true;
	}

	public DateTimeOffset? InTimeZone(
		DateTimeOffset value,
		string ianaId) {
		if (ianaId is null) {
			throw new ArgumentNullException(nameof(ianaId));
		}

		var entry = GetCatalog(null).GetByIanaId(ianaId);

		return entry is null
			? null
			: value.ToInstant().InZone(entry.Zone).ToDateTimeOffset();
	}

	//  ================================================================================
	//  Internals
	//  ================================================================================

	private DateTimeOffset Now => _timeProvider.GetUtcNow();

	private TimeZoneCatalog GetCatalog(
		string? languageCode) {
		languageCode ??= _defaultLanguageCode;

		if (_catalogs.TryGetValue(languageCode, out var cached)) {
			return cached.Value;
		}

		//	TimeZoneNames throws a bare Exception for an unknown language; validate once, before the
		//	catalog is built, so the caller gets an argument error and nothing is cached for it.
		try {
			_ = TZNames.GetNamesForTimeZone("UTC", languageCode);
		} catch (Exception e) when (e is not ArgumentException) {
			throw new ArgumentException($"'{languageCode}' is not a language code TimeZoneNames knows.", nameof(languageCode), e);
		}

		return _catalogs.GetOrAdd(languageCode, code => new Lazy<TimeZoneCatalog>(() => TimeZoneCatalog.Build(_provider, code))).Value;
	}

	private static TimeZone[] Snapshot(
		TimeZoneCatalog.Entry[] entries,
		DateTimeOffset at) {
		var instant = at.ToInstant();
		var timeZones = new TimeZone[entries.Length];

		for (var i = 0; i < entries.Length; i++) {
			timeZones[i] = TimeZoneCatalog.Snapshot(entries[i], instant);
		}

		return timeZones;
	}
}
