// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace BootstrapAdmin.Caching.Services;

class DefaultCacheManager : ICacheManager
{
    [NotNull]
    private IMemoryCache? Cache { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DefaultCacheManager(IMemoryCache cache) => Cache = cache;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public T GetOrAdd<T>(string key, Func<ICacheEntry, T> factory) => Cache.GetOrCreate(key, entry =>
    {
        HandlerEntry(key, entry);
        return factory(entry);
    })!;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> factory) => Cache.GetOrCreate(key, entry =>
    {
        HandlerEntry(key, entry);
        return factory(entry);
    })!;

    private static void HandlerEntry(string key, ICacheEntry entry, IChangeToken? token = null)
    {
        if (token != null)
        {
            entry.AddExpirationToken(token);
        }

        // 内置缓存策略 缓存相对时间 10 分钟
        if (entry.AbsoluteExpiration == null && entry.SlidingExpiration == null && !entry.ExpirationTokens.Any())
        {
#if DEBUG
            entry.SlidingExpiration = TimeSpan.FromMilliseconds(100);
#else
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
#endif
        }
        entry.RegisterPostEvictionCallback((key, value, reason, state) =>
        {

        });
    }

    public void Clear(string? key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            Cache.Remove(key);
        }
        else if (Cache is MemoryCache c)
        {
            c.Compact(100);
        }
    }
}
