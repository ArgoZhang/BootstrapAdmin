using Longbow;
using Longbow.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess.MySQL
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
        public override IEnumerable<DataAccess.Log> RetrieveLogs()
        {
            string sql = "select * from Logs where LogTime > date_add(now(), interval -7 day)";
            List<DataAccess.Log> logs = new List<DataAccess.Log>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    logs.Add(new DataAccess.Log()
                    {
                        Id = reader[0].ToString(),
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
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static void DeleteLogAsync()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                string sql = $"delete from Logs where LogTime < date_add(now(), interval -{ConfigurationManager.AppSettings["KeepLogsPeriod"]} month)";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                DbAccessManager.DBAccess.ExecuteNonQuery(cmd);
            });
        }
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool SaveLog(DataAccess.Log p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            string sql = "Insert Into Logs (CRUD, UserName, LogTime, ClientIp, ClientAgent, RequestUrl) Values (@CRUD, @UserName, now(), @ClientIp, @ClientAgent, @RequestUrl)";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@CRUD", p.CRUD));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserName", p.UserName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ClientIp", p.ClientIp));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ClientAgent", p.ClientAgent));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RequestUrl", p.RequestUrl));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            DeleteLogAsync();
            return ret;
        }
    }
}
