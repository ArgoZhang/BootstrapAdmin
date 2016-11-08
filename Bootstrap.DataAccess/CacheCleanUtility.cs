using Longbow.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    internal static class CacheCleanUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userIds"></param>
        /// <param name="groupIds"></param>
        /// <param name="menuIds"></param>
        /// <param name="dictIds"></param>
        /// <param name="logIds"></param>
        internal static void ClearCache(string roleIds = null, string userIds = null, string groupIds = null, string menuIds = null, string dictIds = null, string logIds = null)
        {
            var cacheKeys = new List<string>();
            if (roleIds != null)
            {
                roleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByRoleIDDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByRoleIDDataKey, id));
                });

                // final cleanup 
                CacheManager.Clear(key => cacheKeys.Any(k => k == key) || key.Contains(RoleHelper.RetrieveRolesDataKey) || key.Contains(MenuHelper.RetrieveMenusDataKey));
                cacheKeys.Clear();
            }
            if (userIds != null)
            {
                userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByUserIDDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByUserIDDataKey, id));
                    cacheKeys.Add(MenuHelper.RetrieveMenusDataKey);
                });
                // final cleanup 
                CacheManager.Clear(key => cacheKeys.Any(k => k == key) || key.Contains(UserHelper.RetrieveUsersDataKey) || key.Contains(RoleHelper.RetrieveRolesByUserNameDataKey));
                cacheKeys.Clear();
            }
            if (groupIds != null)
            {
                groupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByGroupIDDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByGroupIDDataKey, id));
                });
                // final cleanup 
                CacheManager.Clear(key => cacheKeys.Any(k => k == key) || key.Contains(GroupHelper.RetrieveGroupsDataKey) || key.Contains(MenuHelper.RetrieveMenusDataKey));
                cacheKeys.Clear();
            }
            if (menuIds != null)
            {
                menuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByMenuIDDataKey, id));
                });
                // final cleanup 
                CacheManager.Clear(key => cacheKeys.Any(k => k == key) || key.Contains(MenuHelper.RetrieveMenusDataKey));
                cacheKeys.Clear();
            }
            if (dictIds != null)
            {
                // final cleanup 
                CacheManager.Clear(key => key.Contains(DictHelper.RetrieveDictsDataKey));
                cacheKeys.Clear();
            }
            if (logIds != null)
            {
                // final cleanup 
                CacheManager.Clear(key => key.Contains(LogHelper.RetrieveLogsDataKey));
                cacheKeys.Clear();
            }
        }
    }
}
