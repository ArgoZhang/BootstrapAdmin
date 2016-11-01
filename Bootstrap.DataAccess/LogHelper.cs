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
                                    UserID = (int)reader[2],
                                    OperationTime = (DateTime)reader[3],
                                    TableName = (string)reader[4],
                                    BusinessName = (string)reader[5],
                                    PrimaryKey = (string)reader[6],
                                    SqlText = (string)reader[7],
                                    OperationIp = (string)reader[8],
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
            string sql = "Insert Into Logs (OperationType, UserID,OperationTime,TableName,BusinessName,PrimaryKey,SqlText,OperationIp) Values (@OperationType, @UserID,@OperationTime,@TableName,@BusinessName,@PrimaryKey,@SqlText,@OperationIp)";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationType", p.OperationType, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", p.UserID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationTime", p.OperationTime, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@TableName", p.TableName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@BusinessName", p.BusinessName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@PrimaryKey", p.PrimaryKey, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@SqlText", p.SqlText, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@OperationIp", p.OperationIp, ParameterDirection.Input));
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
