using Bootstrap.Security;
using Bootstrap.Security.SQLServer;
using Longbow.Cache;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 用户表相关操作类
    /// </summary>
    public static class UserHelper
    {
        public const string RetrieveUsersByNameDataKey = "BootstrapUser-RetrieveUsersByName";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser RetrieveUserByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName), k => BASQLHelper.RetrieveUserByUserName(userName), RetrieveUsersByNameDataKey);
    }
}
