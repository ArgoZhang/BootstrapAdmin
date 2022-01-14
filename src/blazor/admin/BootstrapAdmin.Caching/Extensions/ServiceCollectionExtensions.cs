using BootstrapAdmin.Caching;
using BootstrapAdmin.Caching.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCacheManager(this IServiceCollection services)
    {
        services.TryAddSingleton<ICacheManager>(provider => DefaultCacheManager.Instance);
        return services;
    }
}
