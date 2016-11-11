using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public class NotificationHelper
    {
        internal const string RetrieveNotifyDataKey = "NotificationHelper-RetrieveNotifications";

        /// <summary>
        /// 查询新注册用户
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUser()
        {
            var ret = UserHelper.RetrieveUsersForNotify();
            if (ret != null)
            {
                ret.AsParallel().ForAll(n =>
                {
                    var ts = DateTime.Now - n.RegisterTime;
                    if (ts.TotalMinutes < 5) n.Period = "刚刚";
                    else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                    else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                    else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
                });
                return ret;
            }
            List<User> users = new List<User>();
            return users;
        }

        /// <summary>
        /// 新用户注册的通知的面板显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Notification> RetrieveNotifications()
        {
            var ret = CacheManager.GetOrAdd(RetrieveNotifyDataKey, CacheSection.RetrieveIntervalByKey(RetrieveNotifyDataKey), key =>
            {
                string sql = "select * from Notifications";
                List<Notification> notifications = new List<Notification>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification()
                            {
                                ID = (int)reader[0],
                                Category = (string)reader[1],
                                Title = (string)reader[2],
                                Content = (string)reader[3],
                                RegisterTime = (DateTime)reader[4],
                                ProcessTime = LgbConvert.ReadValue(reader[5], DateTime.MinValue),
                                ProcessBy = LgbConvert.ReadValue(reader[6], string.Empty),
                                ProcessResult = LgbConvert.ReadValue(reader[7], string.Empty),
                                Status = (string)reader[8]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return notifications;

            }, CacheSection.RetrieveDescByKey(RetrieveNotifyDataKey));
            ret.AsParallel().ForAll(n =>
            {
                var ts = DateTime.Now - n.RegisterTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });
            return ret;
        }

        /// <summary>
        /// 点击某一行用户注册通知的处理成功操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ProcessRegisterUser(string id)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(id)) return ret;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_ProcessRegisterUser"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@id", id, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(notifyIds: id);
                ret = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
    }
}
