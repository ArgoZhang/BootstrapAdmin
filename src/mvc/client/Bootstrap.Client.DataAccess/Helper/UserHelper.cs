using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 用户表相关操作类
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// 通过登录名获取登录用户方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser? RetrieveUserByUserName(string? userName) => string.IsNullOrEmpty(userName) ? null : CacheManager.GetOrAdd(string.Format("{0}-{1}", DbHelper.RetrieveUsersByNameDataKey, userName), k => DbContextManager.Create<User>()?.RetrieveUserByUserName(userName), DbHelper.RetrieveUsersByNameDataKey);
    }
}
