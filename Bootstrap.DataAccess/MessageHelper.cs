using Longbow;
using Longbow.Cache;
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
    public static class MessageHelper
    {
        private const string RetrieveMessageDataKey = "MessageHelper-RetrieveMessages";
        /// <summary>
        /// 所有有关userName所有消息列表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static IEnumerable<Message> RetrieveMessages(string userName)
        {
            var messageRet = CacheManager.GetOrAdd(RetrieveMessageDataKey, key =>
            {
                string sql = "select m.*, d.Name, isnull(i.Code + u.Icon, '~/images/uploader/default.jpg'), u.DisplayName from [Messages] m left join Dicts d on m.Label = d.Code and d.Category = N'消息标签' and d.Define = 0 left join Dicts i on i.Category = N'头像地址' and i.Name = N'头像路径' and i.Define = 0 inner join Users u on m.[From] = u.UserName where [To] = @UserName or [From] = @UserName order by m.SendTime desc";
                List<Message> messages = new List<Message>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", userName));
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        messages.Add(new Message()
                        {
                            Id = (int)reader[0],
                            Title = (string)reader[1],
                            Content = (string)reader[2],
                            From = (string)reader[3],
                            To = (string)reader[4],
                            SendTime = LgbConvert.ReadValue(reader[5], DateTime.MinValue),
                            Status = (string)reader[6],
                            Mark = (int)reader[7],
                            IsDelete = (int)reader[8],
                            Label = (string)reader[9],
                            LabelName = LgbConvert.ReadValue(reader[10], string.Empty),
                            FromIcon = (string)reader[11],
                            FromDisplayName = (string)reader[12]
                        });
                    }
                }
                return messages;

            });
            return messageRet.OrderByDescending(n => n.SendTime);
        }
        /// <summary>
        /// 收件箱
        /// </summary>
        /// <param name="userName"></param>

        public static IEnumerable<Message> Inbox(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.To.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> SendMail(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.From.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 垃圾箱
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Trash(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.IsDelete == 1);
        }
        /// <summary>
        /// 标旗
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Mark(string userName)
        {
            var messageRet = RetrieveMessages(userName);
            return messageRet.Where(n => n.Mark == 1);
        }
        /// <summary>
        /// 获取Header处显示的消息列表
        /// </summary>
        /// <param name="userName"></param>
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
