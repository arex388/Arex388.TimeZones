using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Arex388.TimeZones;

/// <summary>
/// Options for <see cref="ServiceCollectionExtensions.AddTimeZones"/>.
/// </summary>
public sealed class TimeZonesOptions {
	/// <summary>
	/// The language used for <see cref="TimeZone.Name"/> and <see cref="TimeZone.Abbreviation"/> when a call
	/// passes no language code. <c>en-US</c> by default.
	/// </summary>
	public string DefaultLanguageCode { get; set; } = "en-US";

	/// <summary>
	/// The NodaTime provider supplying TZDB. <see cref="DateTimeZoneProviders.Tzdb"/> (the data compiled into the
	/// NodaTime package) by default; set a provider built from a downloaded <c>.nzd</c> file to refresh tzdata
	/// without a package update.
	/// </summary>
	public IDateTimeZoneProvider? DateTimeZoneProvider { get; set; }
}
