using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public class LogHelper
    {
        private const string LogDataKey = "LogData-CodeLogHelper";
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Log> RetrieveLogs(string tId = null)
        {
            string sql = "select * from Logs";
            var ret = CacheManager.GetOrAdd(LogDataKey, CacheSection.RetrieveIntervalByKey(LogDataKey), key =>
                {
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
                                    OperationIp = LgbConvert.ReadValue((string)reader[4],string.Empty),
                                    Remark=LgbConvert.ReadValue((string)reader[5],string.Empty)
                                });
                            }
                        }
                    }
                    catch (Exception ex) { ExceptionManager.Publish(ex); }
                    return Logs;
                }, CacheSection.RetrieveDescByKey(LogDataKey));
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
                    ClearCache();
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
            string sql = "Insert Into Logs (OperationType, UserName,OperationTime,OperationIp,Remark) Values (@OperationType, @UserName,@OperationTime,@OperationIp,@Remark)";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationType", p.OperationType, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", p.UserName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationTime", System.DateTime.Now, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationIp", p.OperationIp, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Remark", p.Remark, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;
                ClearCache();
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        //更新缓存
        private static void ClearCache()
        {
            CacheManager.Clear(key => key == LogDataKey);
        }
    }
}
