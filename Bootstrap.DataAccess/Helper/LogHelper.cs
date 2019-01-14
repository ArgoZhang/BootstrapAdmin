using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveLogsDataKey = "LogHelper-RetrieveLogs";

        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Log> Retrieves() => CacheManager.GetOrAdd(RetrieveLogsDataKey, key => DbContextManager.Create<Log>().Retrieves());

        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Save(Log p)
        {
            var ret = DbContextManager.Create<Log>().Save(p);
            if (ret) CacheManager.Clear(RetrieveLogsDataKey);
            return ret;
        }
    }
}
