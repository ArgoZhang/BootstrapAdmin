using Longbow;
using Longbow.Configuration;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess.SQLite
{
    /// <summary>
    /// 
    /// </summary>
    public class Exceptions : DataAccess.Exceptions
    {
        /// <summary>
        /// 
        /// </summary>
        private static void ClearExceptions()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                string sql = $"delete from Exceptions where LogTime < datetime('now', 'localtime', '-{ConfigurationManager.AppSettings["KeepExceptionsPeriod"]} month')";
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
        public override bool Log(Exception ex, NameValueCollection additionalInfo)
        {
            if (additionalInfo == null)
            {
                additionalInfo = new NameValueCollection
                {
                    ["UserId"] = null,
                    ["UserIp"] = null,
                    ["ErrorPage"] = null
                };
            }
            var errorPage = additionalInfo["ErrorPage"] ?? ex.GetType().Name;
            var sql = "insert into Exceptions (AppDomainName, ErrorPage, UserID, UserIp, ExceptionType, Message, StackTrace, LogTime) values (@AppDomainName, @ErrorPage, @UserID, @UserIp, @ExceptionType, @Message, @StackTrace, datetime('now', 'localtime'))";
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
        public override IEnumerable<DataAccess.Exceptions> RetrieveExceptions()
        {
            string sql = "select * from Exceptions where LogTime > datetime('now', 'localtime', '-7 day') order by LogTime desc";
            List<DataAccess.Exceptions> exceptions = new List<DataAccess.Exceptions>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    exceptions.Add(new DataAccess.Exceptions()
                    {
                        Id = reader[0].ToString(),
                        AppDomainName = (string)reader[1],
                        ErrorPage = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                        UserId = reader.IsDBNull(3) ? string.Empty : (string)reader[3],
                        UserIp = reader.IsDBNull(4) ? string.Empty : (string)reader[4],
                        ExceptionType = (string)reader[5],
                        Message = (string)reader[6],
                        StackTrace = (string)reader[7],
                        LogTime = LgbConvert.ReadValue(reader[8], DateTime.MinValue)
                    });
                }
            }
            return exceptions;
        }
    }
}
