using BootstrapBlazor.Web.Core;
using Microsoft.Extensions.Caching.Memory;

namespace BootstrapAdmin.Web.Core.Services;

class DefaultCacheManager : ICacheManager
{
    private IMemoryCache Cache { get; }

    private List<string> Keys { get; } = new List<string>(256);

    /// <summary>
    /// 
    /// </summary>
    public DefaultCacheManager()
    {
        Cache = new MemoryCache(new MemoryCacheOptions());
    }

    public T GetOrCreate<T>(string key, Func<ICacheEntry, T> factory) => Cache.GetOrCreate(key, entry =>
    {
        HandlerEntry(key, entry);
        return factory(entry);
    });

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> factory) => Cache.GetOrCreate(key, entry =>
    {
        HandlerEntry(key, entry);
        return factory(entry);
    });

    private void HandlerEntry(string key, ICacheEntry entry)
    {
        Keys.Add(key);
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
        entry.RegisterPostEvictionCallback((key, value, reason, state) =>
        {
            var k = key.ToString();
            if (!string.IsNullOrEmpty(k))
            {
                Keys.Remove(k);
            }
        });
    }

    public void Clear(string? key)
    {
        throw new NotImplementedException();
    }

    #region 静态方法
    [NotNull]
    internal static ICacheManager? Instance { get; } = new DefaultCacheManager();
    #endregion
}
