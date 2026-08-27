using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Arex388.TimeZones;

/// <summary>
/// <see cref="IServiceCollection"/> registration for <see cref="ITimeZones"/>.
/// </summary>
public static class ServiceCollectionExtensions {
	/// <summary>
	/// Registers <see cref="ITimeZones"/> as a singleton, if it is not registered already. A registered
	/// <see cref="TimeProvider"/> supplies "now"; otherwise <see cref="TimeProvider.System"/> does.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configure">Optional configuration of the <see cref="TimeZonesOptions"/>.</param>
	/// <returns>The service collection.</returns>
	public static IServiceCollection AddTimeZones(
		this IServiceCollection services,
		Action<TimeZonesOptions>? configure = null) {
		var options = new TimeZonesOptions();

		configure?.Invoke(options);

		services.TryAddSingleton<ITimeZones>(sp => new TimeZones(options, sp.GetService<TimeProvider>() ?? TimeProvider.System));

		return services;
	}
}
