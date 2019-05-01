using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleHelper
    {
        private const string RetrieveRolesByUserNameDataKey = "RoleHelper-RetrieveRolesByUserName";
        private const string RetrieveRolesByUrlDataKey = "RoleHelper-RetrieveRolesByUrl";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUserNameDataKey, userName), key => DbHelper.RetrieveRolesByUserName(userName), RetrieveRolesByUserNameDataKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUrl(string url) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUrlDataKey, url), key => DbHelper.RetrieveRolesByUrl(url), RetrieveRolesByUrlDataKey);
    }
}