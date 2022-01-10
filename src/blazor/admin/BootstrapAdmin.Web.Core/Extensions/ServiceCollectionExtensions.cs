using BootstrapAdmin.Web.Core.Services;
using BootstrapBlazor.Web.Core;
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
