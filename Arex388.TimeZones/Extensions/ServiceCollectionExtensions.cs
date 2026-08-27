using Microsoft.Extensions.DependencyInjection;

namespace Arex388.TimeZones;

/// <summary>
/// IServiceCollection extesnions for TimeZones.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Adds TimeZones service to the service collection as a singleton.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddTimeZones(
        this IServiceCollection services) => services.AddSingleton<ITimeZones, TimeZones>();
}