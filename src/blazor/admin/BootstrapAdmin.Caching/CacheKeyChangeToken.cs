using Microsoft.Extensions.Primitives;

namespace BootstrapAdmin.Caching;

/// <summary>
/// 缓存键值 IChangeToken 实现类
/// </summary>
public class CacheKeyChangeToken : IChangeToken
{
    /// <summary>
    /// 
    /// </summary>
    public bool HasChanged { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool ActiveChangeCallbacks { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IDisposable RegisterChangeCallback(Action<object> callback, object state)
    {
        throw new NotImplementedException();
    }
}
