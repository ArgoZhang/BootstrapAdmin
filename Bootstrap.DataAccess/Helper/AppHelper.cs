using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    public static class AppHelper
    {
        public const string RetrieveAppsByRoleIdDataKey = "AppHelper-RetrieveAppsByRoleId";

        /// <summary>
        /// 根据角色ID指派应用程序
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<App> RetrievesByRoleId(string roleId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveAppsByRoleIdDataKey, roleId), key => DbContextManager.Create<App>().RetrievesByRoleId(roleId), RetrieveAppsByRoleIdDataKey);

        /// <summary>
        /// 根据角色ID以及选定的App ID，保到角色应用表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="appIds"></param>
        /// <returns></returns>
        public static bool SaveByRoleId(string roleId, IEnumerable<string> appIds)
        {
            var ret = DbContextManager.Create<App>().SaveByRoleId(roleId, appIds);
            if (ret) CacheCleanUtility.ClearCache(appIds: appIds, roleIds: new List<string>() { roleId });
            return ret;
        }

        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveAppsByUserName(string userName) => DbContextManager.Create<App>().RetrieveAppsByUserName(userName);
    }
}
