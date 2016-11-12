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


    public class MessageHelper
    {
        internal const string RetrieveMessageFromDataKey = "MessageHelper-RetrieveMessagesFromOthers";
        internal const string RetrieveMessageToDataKey = "MessageHelper-RetrieveMessagesToOthers";
       
        /// <summary>
        /// 获取其他人发送给自己的消息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> RetrieveMessagesFromOthers(string userName)
        {
            var messageRet = CacheManager.GetOrAdd(RetrieveMessageFromDataKey, CacheSection.RetrieveIntervalByKey(RetrieveMessageFromDataKey), key =>
            {
                string sql = "select * from [Messages] where [To]=@UserName";
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
                                Status = (string)reader[6]
                                
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return messages;

            }, CacheSection.RetrieveDescByKey(RetrieveMessageFromDataKey));

            return messageRet.OrderByDescending(n => n.SendTime);
        }

        /// <summary>
        /// 自己获取发送给其他人的消息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> RetrieveMessagesToOthers(string userName)
        {
            var messageRet = CacheManager.GetOrAdd(RetrieveMessageToDataKey, CacheSection.RetrieveIntervalByKey(RetrieveMessageToDataKey), key =>
            {
                string sql = "select * from [Messages] where [From]=@UserName";
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
                                Status = (string)reader[6]

                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return messages;

            }, CacheSection.RetrieveDescByKey(RetrieveMessageToDataKey));

            return messageRet.OrderByDescending(n => n.SendTime);
        }

        /// <summary>
        /// 获取Header处显示的消息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Message> RetrieveMessagesHeader(string userName)
        {

            var messageRet = RetrieveMessagesFromOthers(userName);

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
