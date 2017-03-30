using Longbow.Caching.Configuration;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
        /// <param name="notifyIds"></param>
        /// <param name="exceptionIds"></param>
        internal static void ClearCache(string roleIds = null, string userIds = null, string groupIds = null, string menuIds = null, string dictIds = null, string logIds = null, string notifyIds = null, string exceptionIds = null)
        {
            var cacheKeys = new List<string>();
            if (roleIds != null)
            {
                roleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByRoleIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByRoleIdDataKey, id));
                });
                cacheKeys.Add(RoleHelper.RetrieveRolesDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
            }
            if (userIds != null)
            {
                userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByUserIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", GroupHelper.RetrieveGroupsByUserIdDataKey, id));
                    cacheKeys.Add(MenuHelper.RetrieveMenusDataKey);
                });
                cacheKeys.Add(UserHelper.RetrieveNewUsersDataKey + "*");
                cacheKeys.Add(UserHelper.RetrieveUsersDataKey + "*");
            }
            if (groupIds != null)
            {
                groupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByGroupIdDataKey, id));
                    cacheKeys.Add(string.Format("{0}-{1}", UserHelper.RetrieveUsersByGroupIdDataKey, id));
                });
                cacheKeys.Add(GroupHelper.RetrieveGroupsDataKey + "*");
                cacheKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
            }
            if (menuIds != null)
            {
                menuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(id =>
                {
                    cacheKeys.Add(string.Format("{0}-{1}", RoleHelper.RetrieveRolesByMenuIdDataKey, id));
                });
                cacheKeys.Add(MenuHelper.RetrieveMenusDataKey + "*");
            }
            if (dictIds != null)
            {
                cacheKeys.Add(DictHelper.RetrieveDictsDataKey + "*");
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

            var section = CacheListSection.GetSection();
            section.Items.Where(item => item.Enabled).AsParallel().ForAll(ele =>
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var client = new WebClient();
                        cacheKeys.ForEach(k => client.OpenRead(new Uri(string.Format(ele.Url, k))));

                    }
                    catch (Exception ex)
                    {
                        System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
                        nv["ErrorPage"] = ele.Url;
                        nv["UserId"] = "system";
                        nv["UserIp"] = "::1";
                        ExceptionManager.Publish(ex, nv);
                    }
                });
            });
        }
    }
}
