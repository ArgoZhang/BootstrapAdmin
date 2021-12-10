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
        public static IEnumerable<Message> Inbox(string? userName) => string.IsNullOrEmpty(userName) ? new Message[0] : DbContextManager.Create<Message>()?.Inbox(userName) ?? new Message[0];

        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> SendMail(string? userName) => string.IsNullOrEmpty(userName) ? new Message[0] : DbContextManager.Create<Message>()?.SendMail(userName) ?? new Message[0];

        /// <summary>
        /// 垃圾箱
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Trash(string? userName) => string.IsNullOrEmpty(userName) ? new Message[0] : DbContextManager.Create<Message>()?.Trash(userName) ?? new Message[0];

        /// <summary>
        /// 标旗
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Mark(string? userName) => string.IsNullOrEmpty(userName) ? new Message[0] : DbContextManager.Create<Message>()?.Mark(userName) ?? new Message[0];

        /// <summary>
        /// 获取Header处显示的消息列表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Message> Retrieves(string? userName) => (string.IsNullOrEmpty(userName) ? new Message[0] : CacheManager.GetOrAdd(RetrieveMessageDataKey, key => (DbContextManager.Create<Message>()?.RetrieveHeaders(userName) ?? new Message[0]))).OrderByDescending(n => n.SendTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Save(Message msg) => DbContextManager.Create<Message>()?.Save(msg) ?? false;
    }
}
