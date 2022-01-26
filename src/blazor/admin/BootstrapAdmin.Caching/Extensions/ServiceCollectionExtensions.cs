// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.Caching.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注入 ICacheManager 服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCacheManager(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.TryAddSingleton<ICacheManager>(provider =>
        {
            var cache = provider.GetRequiredService<IMemoryCache>();
            var cacheManager = new DefaultCacheManager(cache);
            CacheManager.Init(cacheManager);
            return cacheManager;
        });
        return services;
    }
}
