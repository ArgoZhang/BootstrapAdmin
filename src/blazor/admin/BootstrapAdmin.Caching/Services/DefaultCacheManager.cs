using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace BootstrapAdmin.Caching.Services;

class DefaultCacheManager : ICacheManager
{
    [NotNull]
    private MemoryCache? Cache { get; set; }

    private List<(string Key, CancellationTokenSource Token)> Keys { get; } = new(256);

    /// <summary>
    /// 
    /// </summary>
    private DefaultCacheManager()
    {
        Init();
    }

    private void Init()
    {
        Keys.Clear();
        Cache = new MemoryCache(new MemoryCacheOptions());
    }

    public T GetOrAdd<T>(string key, Func<ICacheEntry, T> factory) => Cache.GetOrCreate(key, entry =>
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
    public Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> factory) => Cache.GetOrCreate(key, entry =>
    {
        HandlerEntry(key, entry);
        return factory(entry);
    });

    private void HandlerEntry(string key, ICacheEntry entry)
    {
        (string Key, CancellationTokenSource Token) cacheKey = new();
        cacheKey.Key = key;

        if (entry.AbsoluteExpiration == null || entry.SlidingExpiration == null || entry.AbsoluteExpirationRelativeToNow == null)
        {
            // 缓存 10 分钟
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
        }

        if (entry.ExpirationTokens.Count == 0)
        {
            // 增加缓存 Token
            cacheKey.Token = new CancellationTokenSource();
            entry.AddExpirationToken(new CancellationChangeToken(cacheKey.Token.Token));
        }

        entry.RegisterPostEvictionCallback((key, value, reason, state) =>
        {
            // 清理过期缓存键值
            var k = key.ToString();
            if (!string.IsNullOrEmpty(k))
            {
                var item = Keys.LastOrDefault(item => item.Key == k);
                if (item.Key != null)
                {
                    Keys.Remove(item);
                }
            }
        });
        Keys.Add(cacheKey);
    }

    public void Clear(string? key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            var (Key, Token) = Keys.LastOrDefault(item => item.Key == key);
            if (Token != null)
            {
                Token.Cancel();
                Token.Dispose();
            }
        }
        else
        {
            Cache.Compact(100);
        }
    }

    #region 静态方法
    [NotNull]
    internal static ICacheManager? Instance { get; } = new DefaultCacheManager();
    #endregion
}
