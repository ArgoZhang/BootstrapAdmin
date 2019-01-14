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
        public static void ClearCache(IEnumerable<string> roleIds = null, IEnumerable<string> userIds = null, IEnumerable<string> groupIds = null, IEnumerable<string> menuIds = null, IEnumerable<string> dictIds = null, string cacheKey = null)
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
                corsKeys.Add(MenuHelper.RetrieveMenusAll + "*");
            }
            if (userIds != null)
            {
                userIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByUserIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByUserIdDataKey, id));
                    cacheKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                    corsKeys.Add(MenuHelper.RetrieveMenusAll + "*");
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
                cacheKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
            }
            if (menuIds != null)
            {
                menuIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByMenuIdDataKey, id));
                });
                cacheKeys.Add(MenuHelper.RetrieveMenusByRoleIdDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusAll + "*");
                corsKeys.Add(MenuHelper.RetrieveMenusAll + "*");
            }
            if (dictIds != null)
            {
                cacheKeys.Add(DictHelper.RetrieveDictsDataKey + "*");
                cacheKeys.Add(DictHelper.RetrieveCategoryDataKey);
                corsKeys.Add(DictHelper.RetrieveDictsDataKey + "*");
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
