using Longbow.Cache;
using Longbow.Web.WebSockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class NotificationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        internal const string RetrieveNotificationsDataKey = "NotificationHelper-RetrieveNotifications";
        private const string PullNotificationsIntervalDataKey = "NotificationHelper-PullNotificationsInterval";
        private static readonly List<MessageBody> MessagePool = new List<MessageBody>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void PushMessage(MessageBody message)
        {
            MessagePool.Add(message);
            CacheManager.Clear(PullNotificationsIntervalDataKey);

            // websocket message push
            WebSocketServerManager.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new MessageBody[] { message }))));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MessageBody> RetrieveMessages()
        {
            return CacheManager.GetOrAdd(PullNotificationsIntervalDataKey, key =>
            {
                var msgs = new MessageBody[MessagePool.Count];
                MessagePool.CopyTo(msgs, 0);
                MessagePool.Clear();
                return new List<MessageBody>(msgs);
            });
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MessageBody
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}", Category, Message);
        }
    }
}
