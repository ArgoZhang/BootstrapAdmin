using Longbow.Cache;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static IEnumerable<Log> RetrieveLogs(string tId = null)
        {
            var ret = CacheManager.GetOrAdd(RetrieveLogsDataKey, key => DbAdapterManager.Create<Log>().RetrieveLogs(tId));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.Id.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveLog(Log p) => DbAdapterManager.Create<Log>().SaveLog(p);
    }
}
