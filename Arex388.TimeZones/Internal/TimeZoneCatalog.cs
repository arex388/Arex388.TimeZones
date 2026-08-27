using NodaTime;
using NodaTime.TimeZones;
using TimeZoneConverter;
using TimeZoneNames;

namespace Arex388.TimeZones;

/// <summary>
/// The static, per-language table of every TZDB zone with a Windows mapping: ids, the NodaTime zone, and the
/// localized names and abbreviations. Built once per language; the temporal part (offset, daylight-savings
/// state) is computed per call by <see cref="Snapshot"/>.
/// </summary>
internal sealed class TimeZoneCatalog {
	internal sealed class Entry {
		public required string IanaId { get; init; }
		public required string CanonicalIanaId { get; init; }
		public required string WindowsId { get; init; }
		public required DateTimeZone Zone { get; init; }
		public required TimeZoneValues Names { get; init; }
		public required TimeZoneValues Abbreviations { get; init; }
	}

	private readonly Dictionary<string, Entry> _byIanaId;
	private readonly Dictionary<string, Entry[]> _byWindowsId;

	private TimeZoneCatalog(
		Entry[] entries) {
		Entries = entries;
		_byIanaId = entries.ToDictionary(e => e.IanaId, StringComparer.OrdinalIgnoreCase);
		_byWindowsId = entries
			.GroupBy(e => e.WindowsId, StringComparer.OrdinalIgnoreCase)
			.ToDictionary(g => g.Key, g => g.ToArray(), StringComparer.OrdinalIgnoreCase);
	}

	/// <summary>
	/// Every entry, ordered ordinally by IANA id.
	/// </summary>
	public Entry[] Entries { get; }

	public static TimeZoneCatalog Build(
		IDateTimeZoneProvider provider,
		string languageCode) {
		//	provider[alias] returns a zone whose Id is the alias, so the canonical id comes from the
		//	compiled-in source's map; a provider built from newer tzdata falls back to the id itself
		//	for an alias that map does not know.
		var canonicalIds = TzdbDateTimeZoneSource.Default.CanonicalIdMap;
		var entries = new List<Entry>(provider.Ids.Count);

		foreach (var ianaId in provider.Ids) {
			if (!TZConvert.TryIanaToWindows(ianaId, out var windowsId)
				|| string.IsNullOrEmpty(windowsId)) {
				continue;
			}

			var canonicalIanaId = canonicalIds.TryGetValue(ianaId, out var canonical)
				&& provider.GetZoneOrNull(canonical) is not null
				? canonical
				: ianaId;

			entries.Add(new Entry {
				IanaId = ianaId,
				CanonicalIanaId = canonicalIanaId,
				WindowsId = windowsId,
				Zone = provider[ianaId],
				Names = TZNames.GetNamesForTimeZone(ianaId, languageCode),
				Abbreviations = TZNames.GetAbbreviationsForTimeZone(ianaId, languageCode)
			});
		}

		entries.Sort((a, b) => string.CompareOrdinal(a.IanaId, b.IanaId));

		return new TimeZoneCatalog(entries.ToArray());
	}

	public Entry? GetByIanaId(
		string ianaId) => _byIanaId.TryGetValue(ianaId, out var entry)
		? entry
		: null;

	public Entry[] GetByWindowsId(
		string windowsId) => _byWindowsId.TryGetValue(windowsId, out var entries)
		? entries
		: [];

	//  ================================================================================
	//  Snapshot
	//  ================================================================================

	public static TimeZone Snapshot(
		Entry entry,
		Instant instant) {
		var interval = entry.Zone.GetZoneInterval(instant);
		var isDaylightSavings = IsDaylightSavings(entry.Zone, interval);

		return new TimeZone {
			IanaId = entry.IanaId,
			CanonicalIanaId = entry.CanonicalIanaId,
			WindowsId = entry.WindowsId,
			Name = Pick(entry.Names, isDaylightSavings) ?? entry.IanaId,
			Abbreviation = Pick(entry.Abbreviations, isDaylightSavings),
			UtcOffset = interval.WallOffset.ToTimeSpan(),
			IsDaylightSavings = isDaylightSavings,
			At = instant.ToDateTimeOffset()
		};
	}

	/// <summary>
	/// TZDB's savings, normalized for zones that model winter as negative savings: a negative saving is
	/// standard time, a positive one is daylight savings, and a zero saving adjacent to a negative one is the
	/// summer half of a negative-DST zone.
	/// </summary>
	private static bool IsDaylightSavings(
		DateTimeZone zone,
		ZoneInterval interval) {
		if (interval.Savings < Offset.Zero) {
			return false;
		}

		if (interval.Savings > Offset.Zero) {
			return true;
		}

		return interval.HasStart
			&& zone.GetZoneInterval(interval.Start - Duration.Epsilon).Savings < Offset.Zero
			|| interval.HasEnd
			&& zone.GetZoneInterval(interval.End).Savings < Offset.Zero;
	}

	private static string? Pick(
		TimeZoneValues values,
		bool isDaylightSavings) => NullIfEmpty(isDaylightSavings
			? values.Daylight
			: values.Standard)
		?? NullIfEmpty(values.Generic);

	private static string? NullIfEmpty(
		string? value) => string.IsNullOrEmpty(value)
		? null
		: value;
}
