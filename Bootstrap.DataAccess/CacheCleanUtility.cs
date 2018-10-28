using Longbow.Cache;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class CacheCleanUtility
    {
        private const string RetrieveAllRolesDataKey = "BootstrapAdminRoleMiddleware-RetrieveRoles";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userIds"></param>
        /// <param name="groupIds"></param>
        /// <param name="menuIds"></param>
        /// <param name="dictIds"></param>
        /// <param name="cacheKey"></param>
        public static void ClearCache(IEnumerable<int> roleIds = null, IEnumerable<int> userIds = null, IEnumerable<int> groupIds = null, IEnumerable<int> menuIds = null, string dictIds = null, string cacheKey = null)
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
                cacheKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
            }
            if (userIds != null)
            {
                userIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByUserIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByUserIdDataKey, id));
                    cacheKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
                    corsKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
                });
                cacheKeys.Add(UserHelper.RetrieveNewUsersDataKey + "*");
                cacheKeys.Add(UserHelper.RetrieveUsersDataKey + "*");
                corsKeys.Add(UserHelper.RetrieveUsersDataKey + "*");
            }
            if (groupIds != null)
            {
                groupIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByGroupIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByGroupIdDataKey, id));
                });
                cacheKeys.Add(GroupHelper.RetrieveGroupsDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
            }
            if (menuIds != null)
            {
                menuIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByMenuIdDataKey, id));
                });
                cacheKeys.Add(MenuHelper.RetrieveMenusByRoleIdDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
            }
            if (dictIds != null)
            {
                cacheKeys.Add(Dict.RetrieveDictsDataKey + "*");
                cacheKeys.Add(DictHelper.RetrieveCategoryDataKey);
                corsKeys.Add(Dict.RetrieveDictsDataKey + "*");
            }
            if (cacheKey != null)
            {
                cacheKeys.Add(cacheKey);
                corsKeys.Add(cacheKey);
            }
            CacheManager.Clear(cacheKeys);
            CacheManager.CorsClear(corsKeys);
        }
    }
}
