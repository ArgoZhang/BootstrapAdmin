using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 前台应用帮助类
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// 通过角色 ID 获得授权前台应用数据缓存键值
        /// </summary>
        public const string RetrieveAppsByRoleIdDataKey = "AppHelper-RetrieveAppsByRoleId";

        /// <summary>
        /// 通过用户名称获得授权前台应用数据缓存键值
        /// </summary>
        public const string RetrieveAppsByUserNameDataKey = DbHelper.RetrieveAppsByUserNameDataKey;
        /// <summary>
        /// 根据角色ID指派应用程序
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<App> RetrievesByRoleId(string roleId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveAppsByRoleIdDataKey, roleId), key => DbContextManager.Create<App>()?.RetrievesByRoleId(roleId), RetrieveAppsByRoleIdDataKey) ?? new App[0];

        /// <summary>
        /// 根据角色ID以及选定的App ID，保到角色应用表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="appIds"></param>
        /// <returns></returns>
        public static bool SaveByRoleId(string roleId, IEnumerable<string> appIds)
        {
            var ret = DbContextManager.Create<App>()?.SaveByRoleId(roleId, appIds) ?? false;
            if (ret) CacheCleanUtility.ClearCache(appIds: appIds, roleIds: new List<string>() { roleId });
            return ret;
        }

        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUserName(string? userName) => string.IsNullOrEmpty(userName) ? new string[0] : CacheManager.GetOrAdd($"{DbHelper.RetrieveAppsByUserNameDataKey}-{userName}", key => DbContextManager.Create<App>()?.RetrievesByUserName(userName), RetrieveAppsByUserNameDataKey) ?? new string[0];
    }
}
