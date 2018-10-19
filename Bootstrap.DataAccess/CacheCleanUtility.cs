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
                    cacheKeys.Add(string.Format("{0}-{1}", User.RetrieveUsersByRoleIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", Group.RetrieveGroupsByRoleIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", Menu.RetrieveMenusByRoleIdDataKey, id));
                });
                cacheKeys.Add(Role.RetrieveRolesDataKey + "*");
                cacheKeys.Add(Menu.RetrieveMenusDataKey + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
                corsKeys.Add(Menu.RetrieveMenusDataKey + "*");
            }
            if (userIds != null)
            {
                userIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", Role.RetrieveRolesByUserIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", Group.RetrieveGroupsByUserIdDataKey, id));
                    cacheKeys.Add(Menu.RetrieveMenusDataKey + "*");
                    corsKeys.Add(Menu.RetrieveMenusDataKey + "*");
                });
                cacheKeys.Add(User.RetrieveNewUsersDataKey + "*");
                cacheKeys.Add(User.RetrieveUsersDataKey + "*");
                corsKeys.Add(User.RetrieveUsersDataKey + "*");
            }
            if (groupIds != null)
            {
                groupIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", Role.RetrieveRolesByGroupIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", User.RetrieveUsersByGroupIdDataKey, id));
                });
                cacheKeys.Add(Group.RetrieveGroupsDataKey + "*");
                cacheKeys.Add(Menu.RetrieveMenusDataKey + "*");
                corsKeys.Add(Menu.RetrieveMenusDataKey + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
            }
            if (menuIds != null)
            {
                menuIds.ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", Role.RetrieveRolesByMenuIdDataKey, id));
                });
                cacheKeys.Add(Menu.RetrieveMenusByRoleIdDataKey + "*");
                cacheKeys.Add(Menu.RetrieveMenusDataKey + "*");
                corsKeys.Add(Menu.RetrieveMenusDataKey + "*");
            }
            if (dictIds != null)
            {
                cacheKeys.Add(Dict.RetrieveDictsDataKey + "*");
                cacheKeys.Add(Dict.RetrieveCategoryDataKey);
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
