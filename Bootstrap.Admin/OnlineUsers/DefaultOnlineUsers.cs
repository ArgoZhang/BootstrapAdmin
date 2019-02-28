using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    internal class DefaultOnlineUsers : IOnlineUsers
    {
        private ConcurrentDictionary<string, OnlineUser> _onlineUsers = new ConcurrentDictionary<string, OnlineUser>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OnlineUser> OnlineUsers
        {
            get { return _onlineUsers.Values; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="addValueFactory"></param>
        /// <param name="updateValueFactory"></param>
        /// <returns></returns>
        public OnlineUser AddOrUpdate(string key, Func<string, OnlineUser> addValueFactory, Func<string, OnlineUser, OnlineUser> updateValueFactory) => _onlineUsers.AddOrUpdate(key, addValueFactory, updateValueFactory);
    }
}
