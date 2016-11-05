using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Web;

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
            var ret = CacheManager.GetOrAdd(RetrieveLogsDataKey, CacheSection.RetrieveIntervalByKey(RetrieveLogsDataKey), key =>
            {
                string sql = "select * from Logs";
                List<Log> Logs = new List<Log>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Logs.Add(new Log()
                            {
                                ID = (int)reader[0],
                                OperationType = (int)reader[1],
                                UserName = (string)reader[2],
                                OperationTime = (DateTime)reader[3],
                                OperationIp = (string)reader[4],
                                Remark = (string)reader[5],
                                OperationModule = (string)reader[6]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Logs;
            }, CacheSection.RetrieveDescByKey(RetrieveLogsDataKey));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.ID.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DeleteLog(string ids)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
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
            string sql = "Insert Into Logs (OperationType, UserName,OperationTime,OperationIp,Remark,OperationModule) Values (@OperationType, @UserName,@OperationTime,@OperationIp,@Remark,@OperationModule)";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationType", p.OperationType, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", p.UserName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationTime", p.OperationTime, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationIp", p.OperationIp == null ? "" : p.OperationIp, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Remark", p.Remark == null ? "" : p.Remark, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationModule", p.OperationModule == null ? "" : p.OperationModule, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(logIds: p.ID == 0 ? "" : p.ID.ToString());
                ret = true;
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            HttpRequest request = HttpContext.Current.Request;
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = request.UserHostAddress;
            if (string.IsNullOrEmpty(result))
                result = "0.0.0.0";
            return result;
        }
    }
}
