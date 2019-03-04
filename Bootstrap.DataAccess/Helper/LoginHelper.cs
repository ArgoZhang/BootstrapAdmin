using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoginHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveLoginLogsDataKey = "LoginHelper-Retrieves";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Log(LoginUser user)
        {
            var ret = DbContextManager.Create<LoginUser>().Log(user);
            if (ret) CacheManager.Clear(RetrieveLoginLogsDataKey);
            return ret;
        }

        /// <summary>
        /// 查询一个月内所有登录信息
        /// </summary>
        public static IEnumerable<LoginUser> Retrieves() => CacheManager.GetOrAdd(RetrieveLoginLogsDataKey, key => DbContextManager.Create<LoginUser>().Retrieves());
    }
}
