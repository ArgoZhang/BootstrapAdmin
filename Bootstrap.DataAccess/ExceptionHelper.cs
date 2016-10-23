using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
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
        internal const string RetrieveExceptionsDataKey = "ExceptionHelper-RetrieveExceptions";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public static bool Log(Exception ex, NameValueCollection additionalInfo)
        {
            bool ret = false;
            try
            {
                var sql = "insert into Exceptions (AppDomainName, ErrorPage, UserID, UserIp, ExceptionType, Message, StackTrace, LogTime) values (@AppDomainName, @ErrorPage, @UserID, @UserIp, @ExceptionType, @Message, @StackTrace, GetDate())";
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@AppDomainName", AppDomain.CurrentDomain.FriendlyName));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ErrorPage", additionalInfo["ErrorPage"]));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", DBAccess.ToDBValue(additionalInfo["UserId"])));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserIp", additionalInfo["UserIp"]));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ExceptionType", ex.GetType().FullName));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Message", ex.Message));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@StackTrace", DBAccess.ToDBValue(ex.StackTrace), ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;
            }
            catch (Exception e)
            {
                throw new Exception("Excetion Log Error", e);
            }
            return ret;
        }
        /// <summary>
        /// 查询所有异常
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Exceptions> RetrieveExceptions()
        {
            return CacheManager.GetOrAdd(RetrieveExceptionsDataKey, CacheSection.RetrieveIntervalByKey(RetrieveExceptionsDataKey), key =>
            {
                string sql = "select top 1000 * from Exceptions order by LogTime desc";
                List<Exceptions> exceptions = new List<Exceptions>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            exceptions.Add(new Exceptions()
                            {
                                Id = (int)reader[0],
                                AppDomainName = (string)reader[1],
                                ErrorPage = (string)reader[2],
                                UserId = (string)reader[3],
                                UserIp = (string)reader[4],
                                ExceptionType = (string)reader[5],
                                Message = (string)reader[6],
                                StackTrace = (string)reader[7],
                                LogTime = (DateTime)reader[8],
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return exceptions;
            }, CacheSection.RetrieveDescByKey(RetrieveExceptionsDataKey));
        }
    }
}
