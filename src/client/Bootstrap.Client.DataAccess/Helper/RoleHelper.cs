using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
using System;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", DbHelper.RetrieveRolesByUserNameDataKey, userName), key => DbContextManager.Create<Role>()?.RetrievesByUserName(userName), DbHelper.RetrieveRolesByUserNameDataKey) ?? Array.Empty<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUrl(string url, string appId) => CacheManager.GetOrAdd(string.Format("{0}-{1}-{2}", DbHelper.RetrieveRolesByUrlDataKey, url, appId), key => DbContextManager.Create<Role>()?.RetrievesByUrl(url, appId), DbHelper.RetrieveRolesByUrlDataKey) ?? Array.Empty<string>();
    }
}
