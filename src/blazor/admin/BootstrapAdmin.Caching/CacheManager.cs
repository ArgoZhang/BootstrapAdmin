// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching.Services;
using Microsoft.Extensions.Caching.Memory;

namespace BootstrapAdmin.Caching;

/// <summary>
/// 缓存管理类
/// </summary>
public static class CacheManager
{
    [NotNull]
    private static ICacheManager? Cache { get; set; }

    /// <summary>
    /// 由服务调用
    /// </summary>
    /// <param name="cache"></param>
    internal static void Init(ICacheManager cache) => Cache = cache;

    /// <summary>
    /// 获得或者新建数据
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static TItem GetOrAdd<TItem>(string key, Func<ICacheEntry, TItem> valueFactory) => Cache.GetOrAdd(key, valueFactory);

    /// <summary>
    /// 清除指定键值缓存项
    /// </summary>
    /// <param name="key"></param>
    public static void Clear(string? key = null) => Cache.Clear(key);
}
