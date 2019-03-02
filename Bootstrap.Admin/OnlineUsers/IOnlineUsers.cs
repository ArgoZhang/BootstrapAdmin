using System;
using System.Collections.Generic;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOnlineUsers
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<OnlineUser> OnlineUsers { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="addValueFactory"></param>
        /// <param name="updateValueFactory"></param>
        /// <returns></returns>
        OnlineUserCache AddOrUpdate(string key, Func<string, OnlineUserCache> addValueFactory, Func<string, OnlineUserCache, OnlineUserCache> updateValueFactory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onlineUserCache"></param>
        /// <returns></returns>
        bool TryRemove(string key, out OnlineUserCache onlineUserCache);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        string RetrieveLocaleByIp(string ip = null);
    }
}
