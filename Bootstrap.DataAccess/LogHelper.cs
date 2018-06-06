using Longbow.Cache;
using Longbow.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
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
                string sql = "select top 1000 * from Logs";
                List<Log> logs = new List<Log>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
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
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return logs;
            });
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.Id.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DeleteLog(string ids)
        {
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return false;
            bool ret = false;
            try
            {
                string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Logs where ID in ({0})", ids);
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                    CacheCleanUtility.ClearCache(logIds: ids);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
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
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@CRUD", p.CRUD));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", p.UserName));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ClientIp", p.ClientIp));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ClientAgent", p.ClientAgent));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RequestUrl", p.RequestUrl));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(logIds: p.Id == 0 ? string.Empty : p.Id.ToString());
                ret = true;
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
    }
}
