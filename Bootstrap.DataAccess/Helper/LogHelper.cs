using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    public static class LogHelper
    {
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Log> RetrieveLogs(string tId = null) => DbAdapterManager.Create<Log>().RetrieveLogs(tId);
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveLog(Log p) => DbAdapterManager.Create<Log>().SaveLog(p);
    }
}
