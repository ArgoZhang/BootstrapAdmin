using BootstrapAdmin.Web.Core.Services;
using BootstrapBlazor.Web.Core;
using Microsoft.Extensions.Caching.Memory;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 缓存管理类
/// </summary>
public static class CacheManager
{
    private static ICacheManager Cache { get; } = DefaultCacheManager.Instance;

    /// <summary>
    /// 获得或者新建数据
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static TItem GetOrCreate<TItem>(string key, Func<ICacheEntry, TItem> valueFactory) => Cache.GetOrCreate(key, valueFactory);

    /// <summary>
    /// 清除指定键值缓存项
    /// </summary>
    /// <param name="key"></param>
    public static void Clear(string? key) => Cache.Clear(key);
}
