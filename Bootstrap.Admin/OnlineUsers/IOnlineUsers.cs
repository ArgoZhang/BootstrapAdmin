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
        OnlineUser AddOrUpdate(string key, Func<string, OnlineUser> addValueFactory, Func<string, OnlineUser, OnlineUser> updateValueFactory);
    }
}
