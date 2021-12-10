using Longbow.Cache;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 缓存清理操作类
    /// </summary>
    public static class CacheCleanUtility
    {
        private const string RetrieveAllRolesDataKey = "BootstrapAdminRoleMiddleware-RetrieveRoles";
        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userIds"></param>
        /// <param name="groupIds"></param>
        /// <param name="menuIds"></param>
        /// <param name="appIds"></param>
        /// <param name="dictIds"></param>
        /// <param name="cacheKey"></param>
        public static void ClearCache(IEnumerable<string>? roleIds = null, IEnumerable<string>? userIds = null, IEnumerable<string>? groupIds = null, IEnumerable<string>? menuIds = null, IEnumerable<string>? appIds = null, IEnumerable<string>? dictIds = null, string? cacheKey = null)
        {
            var cacheKeys = new List<string>();
            var corsKeys = new List<string>();
            if (roleIds != null)
            {
                roleIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByRoleIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByRoleIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", MenuHelper.RetrieveMenusByRoleIdDataKey, id));
                });
                cacheKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
                corsKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                corsKeys.Add(GroupHelper.RetrieveGroupsDataKey + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusAll + "*");
            }
            if (userIds != null)
            {
                userIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByUserIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByUserIdDataKey, id));
                    cacheKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                });
                cacheKeys.Add(UserHelper.RetrieveNewUsersDataKey + "*");
                cacheKeys.Add(UserHelper.RetrieveUsersDataKey);
                cacheKeys.Add(GroupHelper.RetrieveGroupsByUserNameDataKey + "*");
                cacheKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                cacheKeys.Add(AppHelper.RetrieveAppsByUserNameDataKey + "*");
                corsKeys.Add(UserHelper.RetrieveUsersDataKey);
                corsKeys.Add(GroupHelper.RetrieveGroupsByUserNameDataKey + "*");
                corsKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                corsKeys.Add(AppHelper.RetrieveAppsByUserNameDataKey + "*");
            }
            if (groupIds != null)
            {
                groupIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByGroupIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByGroupIdDataKey, id));
                });
                cacheKeys.Add(GroupHelper.RetrieveGroupsDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
                corsKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                corsKeys.Add(GroupHelper.RetrieveGroupsDataKey + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusAll + "*");
            }
            if (menuIds != null)
            {
                menuIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByMenuIdDataKey, id));
                });
                cacheKeys.Add(MenuHelper.RetrieveMenusByRoleIdDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                corsKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                corsKeys.Add(GroupHelper.RetrieveGroupsDataKey + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusAll + "*");
            }
            if (appIds != null)
            {
                cacheKeys.Add("AppHelper-RetrieveAppsBy*");
                corsKeys.Add("AppHelper-RetrieveAppsBy*");
            }
            if (dictIds != null)
            {
                cacheKeys.Add(AppHelper.RetrieveAppsByUserNameDataKey + "*");
                cacheKeys.Add(DictHelper.RetrieveDictsDataKey);
                cacheKeys.Add(DictHelper.RetrieveCategoryDataKey);
                corsKeys.Add(DictHelper.RetrieveDictsDataKey);
                corsKeys.Add(AppHelper.RetrieveAppsByUserNameDataKey + "*");
            }
            if (cacheKey != null)
            {
                cacheKeys.Add(cacheKey);
                corsKeys.Add(cacheKey);
            }
            CacheManager.Clear(cacheKeys);
            CacheManager.CorsClear(corsKeys.Distinct());
        }
    }
}
