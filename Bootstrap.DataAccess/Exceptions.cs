using Longbow.Configuration;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Exceptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppDomainName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LogTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExceptionType { get; set; }
        /// <summary>
        /// 获得/设置 时间描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }

        private static void ClearExceptions()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                string sql = $"delete from Exceptions where LogTime < DATEADD(MONTH, -{ConfigurationManager.AppSettings["KeepExceptionsPeriod"]}, GETDATE())";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                DbAccessManager.DBAccess.ExecuteNonQuery(cmd);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public virtual bool Log(Exception ex, NameValueCollection additionalInfo)
        {
            if (ex == null) return true;
            if (additionalInfo == null)
            {
                additionalInfo = new NameValueCollection
                {
                    ["UserId"] = null,
                    ["UserIp"] = null,
                    ["ErrorPage"] = null
                };
            }
            var errorPage = additionalInfo["ErrorPage"] ?? (ex.GetType().Name.Length > 50 ? ex.GetType().Name.Substring(0, 50) : ex.GetType().Name);
            var sql = "insert into Exceptions (AppDomainName, ErrorPage, UserID, UserIp, ExceptionType, Message, StackTrace, LogTime) values (@AppDomainName, @ErrorPage, @UserID, @UserIp, @ExceptionType, @Message, @StackTrace, GetDate())";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@AppDomainName", AppDomain.CurrentDomain.FriendlyName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ErrorPage", errorPage));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserID", DbAdapterManager.ToDBValue(additionalInfo["UserId"])));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserIp", DbAdapterManager.ToDBValue(additionalInfo["UserIp"])));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ExceptionType", ex.GetType().FullName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Message", ex.Message));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@StackTrace", DbAdapterManager.ToDBValue(ex.StackTrace)));
                DbAccessManager.DBAccess.ExecuteNonQuery(cmd);
                ClearExceptions();
            }
            return true;
        }
        /// <summary>
        /// 查询一周内所有异常
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Exceptions> RetrieveExceptions()
        {
            string sql = "select * from Exceptions where DATEDIFF(Week, LogTime, GETDATE()) = 0 order by LogTime desc";
            List<Exceptions> exceptions = new List<Exceptions>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    exceptions.Add(new Exceptions()
                    {
                        Id = reader[0].ToString(),
                        AppDomainName = (string)reader[1],
                        ErrorPage = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                        UserId = reader.IsDBNull(3) ? string.Empty : (string)reader[3],
                        UserIp = reader.IsDBNull(4) ? string.Empty : (string)reader[4],
                        ExceptionType = (string)reader[5],
                        Message = (string)reader[6],
                        StackTrace = reader.IsDBNull(7) ? string.Empty : (string)reader[7],
                        LogTime = (DateTime)reader[8],
                    });
                }
            }
            return exceptions;
        }
    }
}
