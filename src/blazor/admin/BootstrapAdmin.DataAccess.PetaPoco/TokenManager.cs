using BootstrapAdmin.DataAccess.PetaPoco.Services;
using Microsoft.Extensions.Primitives;

namespace BootstrapAdmin.Caching;

/// <summary>
/// 
/// </summary>
public static class TokenManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IChangeToken GetOrAdd(string key)
    {
        IChangeToken? token = null;
        if (key.StartsWith($"{nameof(NavigationService)}-"))
        {
            // 菜单需要更新
            //token = new CompositeChangeToken();
        }
        return token;
    }
}
