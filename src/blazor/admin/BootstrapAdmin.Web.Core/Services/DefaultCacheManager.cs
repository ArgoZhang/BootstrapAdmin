using BootstrapBlazor.Web.Core;
using Microsoft.Extensions.Caching.Memory;

namespace BootstrapAdmin.Web.Core.Services;

class DefaultCacheManager : ICacheManager
{
    private static readonly Lazy<ICacheManager> cache = new(() => new DefaultCacheManager());

    public static ICacheManager Instance { get; } = cache.Value;

    public T GetOrCreate<T>(string key, Func<ICacheEntry, T> factory)
    {
        throw new NotImplementedException();
    }

    public void Clear(string? key)
    {
        throw new NotImplementedException();
    }
}
