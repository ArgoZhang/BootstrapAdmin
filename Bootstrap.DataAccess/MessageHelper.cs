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
    /// <summary>
    /// 
    /// </summary>
    public class MessageHelper
    {
        internal const string RetrieveMessageDataKey = "MessageHelper-RetrieveMessages";


        /// <summary>
        /// 所有有关userName所有消息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> RetrieveMessages(string userName)
        {
            var messageRet = CacheManager.GetOrAdd(RetrieveMessageDataKey, CacheSection.RetrieveIntervalByKey(RetrieveMessageDataKey), key =>
            {
                string sql = "select * from [Messages] where [To]=@UserName or [From]=@UserName";
                List<Message> messages = new List<Message>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", userName, ParameterDirection.Input));
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            messages.Add(new Message()
                            {
                                ID = (int)reader[0],
                                Title = (string)reader[1],
                                Content = (string)reader[2],
                                From = (string)reader[3],
                                To = (string)reader[4],
                                SendTime = LgbConvert.ReadValue(reader[5], DateTime.MinValue),
                                Status = (string)reader[6],
                                Mark=(int)reader[7],
                                IsDelete=(int)reader[8],
                                Label=(string)reader[9]

                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return messages;

            }, CacheSection.RetrieveDescByKey(RetrieveMessageDataKey));
            return messageRet.OrderByDescending(n => n.SendTime);
        }
        /// <summary>
        /// 收件箱
        /// </summary>
        /// <param name="id"></param>
        
        public static IEnumerable<Message> Inbox(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.To.Equals(userName)).Select(n => n);
        }
        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> SendMail(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.To.Equals(userName)).Select(n => n);
        }
        /// <summary>
        /// 垃圾箱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Trash(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.Trash==1).Select(n => n);
        }
        /// <summary>
        /// 标旗
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Mark(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.Mark==1).Select(n => n);
        }
        /// <summary>
        /// 获取Header处显示的消息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> RetrieveMessagesHeader(string userName)
        {
            var messageRet = Inbox(userName);
            messageRet.AsParallel().ForAll(n =>
            {
                var ts = DateTime.Now - n.SendTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });
            return messageRet;
        }
    }
}
