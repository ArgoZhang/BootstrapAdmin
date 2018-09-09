using Longbow.Cache;
using Longbow.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public static class LogHelper
    {
        internal const string RetrieveLogsDataKey = "LogHelper-RetrieveLogs";
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Log> RetrieveLogs(string tId = null)
        {
            var ret = CacheManager.GetOrAdd(RetrieveLogsDataKey, key =>
            {
                string sql = "select * from Logs where DATEDIFF(Week, LogTime, GETDATE()) = 0";
                List<Log> logs = new List<Log>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        logs.Add(new Log()
                        {
                            Id = (int)reader[0],
                            CRUD = (string)reader[1],
                            UserName = (string)reader[2],
                            LogTime = (DateTime)reader[3],
                            ClientIp = (string)reader[4],
                            ClientAgent = (string)reader[5],
                            RequestUrl = (string)reader[6]
                        });
                    }
                }
                return logs;
            });
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.Id.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void DeleteLogAsync()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                string sql = $"delete from Logs where LogTime < DATEADD(MONTH, -{ConfigurationManager.AppSettings["KeepLogsPeriod"]}, GETDATE())";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            });
        }
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveLog(Log p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            string sql = "Insert Into Logs (CRUD, UserName, LogTime, ClientIp, ClientAgent, RequestUrl) Values (@CRUD, @UserName, GetDate(), @ClientIp, @ClientAgent, @RequestUrl)";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@CRUD", p.CRUD));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", p.UserName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ClientIp", p.ClientIp));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ClientAgent", p.ClientAgent));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RequestUrl", p.RequestUrl));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(logIds: p.Id == 0 ? string.Empty : p.Id.ToString());
            DeleteLogAsync();
            return ret;
        }
    }
}
