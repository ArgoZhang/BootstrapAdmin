using Longbow.Cache;
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
    public static class ExceptionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly string RetrieveExceptionsDataKey = "ExceptionHelper-RetrieveExceptions";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public static void Log(Exception ex, NameValueCollection additionalInfo)
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
            var errorPage = additionalInfo["ErrorPage"] ?? (nameof(ex).Length > 50 ? nameof(ex).Substring(0, 50) : nameof(ex));
            var sql = "insert into Exceptions (AppDomainName, ErrorPage, UserID, UserIp, ExceptionType, Message, StackTrace, LogTime) values (@AppDomainName, @ErrorPage, @UserID, @UserIp, @ExceptionType, @Message, @StackTrace, GetDate())";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@AppDomainName", AppDomain.CurrentDomain.FriendlyName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ErrorPage", errorPage));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", DBAccessFactory.ToDBValue(additionalInfo["UserId"])));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserIp", DBAccessFactory.ToDBValue(additionalInfo["UserIp"])));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ExceptionType", ex.GetType().FullName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Message", ex.Message));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@StackTrace", DBAccessFactory.ToDBValue(ex.StackTrace)));
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                CacheManager.Clear(RetrieveExceptionsDataKey);
                ClearExceptions();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private static void ClearExceptions()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                string sql = $"delete from Exceptions where LogTime < DATEADD(MONTH, -{ConfigurationManager.AppSettings["KeepExceptionsPeriod"]}, GETDATE())";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            });
        }
        /// <summary>
        /// 查询一周内所有异常
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Exceptions> RetrieveExceptions()
        {
            return CacheManager.GetOrAdd(RetrieveExceptionsDataKey, key =>
            {
                string sql = "select * from Exceptions where DATEDIFF(Week, LogTime, GETDATE()) = 0 order by LogTime desc";
                List<Exceptions> exceptions = new List<Exceptions>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        exceptions.Add(new Exceptions()
                        {
                            Id = (int)reader[0],
                            AppDomainName = (string)reader[1],
                            ErrorPage = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            UserId = reader.IsDBNull(3) ? string.Empty : (string)reader[3],
                            UserIp = reader.IsDBNull(4) ? string.Empty : (string)reader[4],
                            ExceptionType = (string)reader[5],
                            Message = (string)reader[6],
                            StackTrace = (string)reader[7],
                            LogTime = (DateTime)reader[8],
                        });
                    }
                }
                return exceptions;
            });
        }
    }
}
