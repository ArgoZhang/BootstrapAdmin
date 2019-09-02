using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
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
        public static IEnumerable<string> RetrievesByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", DbHelper.RetrieveRolesByUserNameDataKey, userName), key => DbContextManager.Create<Role>().RetrievesByUserName(userName), DbHelper.RetrieveRolesByUserNameDataKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUrl(string url) => CacheManager.GetOrAdd(string.Format("{0}-{1}", DbHelper.RetrieveRolesByUrlDataKey, url), key => DbContextManager.Create<Role>().RetrievesByUrl(url), DbHelper.RetrieveRolesByUrlDataKey);
    }
}
