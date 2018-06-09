using Bootstrap.Security;
using Longbow.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    internal static class CacheCleanUtility
    {
        const string RetrieveAllRolesDataKey = "BootstrapAdminRoleMiddleware-RetrieveRoles";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userIds"></param>
        /// <param name="groupIds"></param>
        /// <param name="menuIds"></param>
        /// <param name="dictIds"></param>
        /// <param name="logIds"></param>
        /// <param name="notifyIds"></param>
        /// <param name="exceptionIds"></param>
        internal static void ClearCache(string roleIds = null, string userIds = null, string groupIds = null, string menuIds = null, string dictIds = null, string logIds = null, string notifyIds = null, string exceptionIds = null, string cacheKey = null)
        {
            var cacheKeys = new List<string>();
            var corsKeys = new List<string>();
            if (roleIds != null)
            {
                roleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByRoleIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByRoleIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", MenuHelper.RetrieveMenusByRoleIdDataKey, id));
                });
                cacheKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                cacheKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
                corsKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
            }
            if (userIds != null)
            {
                userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByUserIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByUserIdDataKey, id));
                    cacheKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
                    corsKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
                });
                cacheKeys.Add(UserHelper.RetrieveNewUsersDataKey + "*");
                cacheKeys.Add(BootstrapUser.RetrieveUsersDataKey + "*");
                corsKeys.Add(BootstrapUser.RetrieveUsersDataKey + "*");
            }
            if (groupIds != null)
            {
                groupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByGroupIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByGroupIdDataKey, id));
                });
                cacheKeys.Add(GroupHelper.RetrieveGroupsDataKey + "*");
                cacheKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
                corsKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
                cacheKeys.Add(RetrieveAllRolesDataKey + "*");
            }
            if (menuIds != null)
            {
                menuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByMenuIdDataKey, id));
                });
                cacheKeys.Add(MenuHelper.RetrieveMenusByRoleIdDataKey + "*");
                cacheKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
                corsKeys.Add(BootstrapMenu.RetrieveMenusDataKey + "*");
            }
            if (dictIds != null)
            {
                cacheKeys.Add(BootstrapDict.RetrieveDictsDataKey + "*");
                corsKeys.Add(BootstrapDict.RetrieveDictsDataKey + "*");
            }
            if (logIds != null)
            {
                cacheKeys.Add(LogHelper.RetrieveLogsDataKey + "*");
            }
            if (notifyIds != null)
            {
                cacheKeys.Add(NotificationHelper.RetrieveNotificationsDataKey + "*");
            }
            if (exceptionIds != null) 
            {
                cacheKeys.Add(ExceptionHelper.RetrieveExceptionsDataKey + "*");
            }
            if (cacheKey != null)
            {
                cacheKeys.Add(cacheKey);
            }
            CacheManager.Clear(cacheKeys);
            CacheManager.CorsClear(corsKeys);
        }
    }
}
