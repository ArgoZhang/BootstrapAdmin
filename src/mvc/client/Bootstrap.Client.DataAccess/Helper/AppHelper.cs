using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 应用操作帮助类
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUserName(string userName) => CacheManager.GetOrAdd($"{DbHelper.RetrieveAppsByUserNameDataKey}-{userName}", key => DbContextManager.Create<App>()?.RetrievesByUserName(userName), DbHelper.RetrieveAppsByUserNameDataKey) ?? new string[0];
    }
}
