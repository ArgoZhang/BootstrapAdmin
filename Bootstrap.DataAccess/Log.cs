using Longbow.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 获得/设置 操作日志主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获得/设置 操作类型
        /// </summary>
        public string CRUD { get; set; }

        /// <summary>
        /// 获得/设置 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获得/设置 操作时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 获得/设置 客户端IP
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 获取/设置 客户端信息
        /// </summary>
        public string ClientAgent { get; set; }

        /// <summary>
        /// 获取/设置 请求网址
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Log> RetrieveLogs()
        {
            string sql = "select * from Logs where DATEDIFF(Week, LogTime, GETDATE()) = 0";
            List<Log> logs = new List<Log>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    logs.Add(new Log()
                    {
                        Id = reader[0].ToString(),
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
                string sql = $"delete from Logs where LogTime < DATEADD(MONTH, -{ConfigurationManager.AppSettings["KeepLogsPeriod"]}, GETDATE())";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                DbAccessManager.DBAccess.ExecuteNonQuery(cmd);
            });
        }
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveLog(Log p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            string sql = "Insert Into Logs (CRUD, UserName, LogTime, ClientIp, ClientAgent, RequestUrl) Values (@CRUD, @UserName, GetDate(), @ClientIp, @ClientAgent, @RequestUrl)";
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
