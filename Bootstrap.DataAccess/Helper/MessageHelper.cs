using Longbow.Cache;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveMessageDataKey = "MessageHelper-RetrieveMessages";

        /// <summary>
        /// 收件箱
        /// </summary>
        /// <param name="userName"></param>
        public static IEnumerable<Message> Inbox(string userName) => DbContextManager.Create<Message>().Inbox(userName);

        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> SendMail(string userName) => DbContextManager.Create<Message>().SendMail(userName);

        /// <summary>
        /// 垃圾箱
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Trash(string userName) => DbContextManager.Create<Message>().Trash(userName);

        /// <summary>
        /// 标旗
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Mark(string userName) => DbContextManager.Create<Message>().Mark(userName);

        /// <summary>
        /// 获取Header处显示的消息列表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Retrieves(string userName) => CacheManager.GetOrAdd(RetrieveMessageDataKey, key => DbContextManager.Create<Message>().RetrieveHeaders(userName).OrderByDescending(n => n.SendTime));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Save(Message msg) => DbContextManager.Create<Message>().Save(msg);
    }
}
