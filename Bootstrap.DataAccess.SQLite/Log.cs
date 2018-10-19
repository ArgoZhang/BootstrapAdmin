using Longbow;
using Longbow.Cache;
using Longbow.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.SQLite
{
    /// <summary>
    /// 
    /// </summary>
    public class Log : DataAccess.Log
    {
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Log> RetrieveLogs(string tId = null)
        {
            var ret = CacheManager.GetOrAdd(RetrieveLogsDataKey, key =>
            {
                string sql = "select * from Logs where LogTime > datetime('now', 'localtime', '-7 day')";
                List<Log> logs = new List<Log>();
                DbCommand cmd = DBAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DBAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        logs.Add(new Log()
                        {
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            CRUD = (string)reader[1],
                            UserName = (string)reader[2],
                            LogTime = LgbConvert.ReadValue(reader[3], DateTime.MinValue),
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
        private void DeleteLogAsync()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                string sql = $"delete from Logs where LogTime < datetime('now', 'localtime', '-{ConfigurationManager.AppSettings["KeepLogsPeriod"]} month')";
                DbCommand cmd = DBAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                DBAccessManager.DBAccess.ExecuteNonQuery(cmd);
            });
        }
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool SaveLog(Bootstrap.DataAccess.Log p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            string sql = "Insert Into Logs (CRUD, UserName, LogTime, ClientIp, ClientAgent, RequestUrl) Values (@CRUD, @UserName, datetime('now', 'localtime'), @ClientIp, @ClientAgent, @RequestUrl)";
            using (DbCommand cmd = DBAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@CRUD", p.CRUD));
                cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@UserName", p.UserName));
                cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@ClientIp", p.ClientIp));
                cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@ClientAgent", p.ClientAgent));
                cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@RequestUrl", p.RequestUrl));
                ret = DBAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheManager.Clear(RetrieveLogsDataKey);
            DeleteLogAsync();
            return ret;
        }
    }
}
