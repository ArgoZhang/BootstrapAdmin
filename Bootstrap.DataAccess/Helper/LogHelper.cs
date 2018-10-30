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
        public static IEnumerable<Log> RetrieveLogs() => CacheManager.GetOrAdd(RetrieveLogsDataKey, key => DbAdapterManager.Create<Log>().RetrieveLogs());
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveLog(Log p)
        {
            var ret = DbAdapterManager.Create<Log>().SaveLog(p);
            if (ret) CacheManager.Clear(RetrieveLogsDataKey);
            return ret;
        }
    }
}
