using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class NotificationsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Notifications> RetrieveNotifications()
        {
            var ret = new List<Notifications>();
            ret.Add(new Notifications() { Title = "测试消息1", Content = "用户注册", RegisterTime = DateTime.Now });
            ret.Add(new Notifications() { Title = "测试消息2", Content = "用户注册", RegisterTime = DateTime.Now });
            return ret;
        }
    }
}
