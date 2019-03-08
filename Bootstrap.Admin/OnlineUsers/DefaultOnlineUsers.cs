using Bootstrap.DataAccess;
using Longbow.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    internal class DefaultOnlineUsers : IOnlineUsers
    {
        private ConcurrentDictionary<string, OnlineUserCache> _onlineUsers = new ConcurrentDictionary<string, OnlineUserCache>();
        private HttpClient _client;
        private IEnumerable<string> _local = new string[] { "::1", "127.0.0.1" };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        public DefaultOnlineUsers(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(OnlineUsersServicesCollectionExtensions.IPSvrHttpClientName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OnlineUser> OnlineUsers
        {
            get { return _onlineUsers.Values.Select(v => v.User); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="addValueFactory"></param>
        /// <param name="updateValueFactory"></param>
        /// <returns></returns>
        public OnlineUserCache AddOrUpdate(string key, Func<string, OnlineUserCache> addValueFactory, Func<string, OnlineUserCache, OnlineUserCache> updateValueFactory) => _onlineUsers.AddOrUpdate(key, addValueFactory, updateValueFactory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onlineUserCache"></param>
        /// <returns></returns>
        public bool TryRemove(string key, out OnlineUserCache onlineUserCache) => _onlineUsers.TryRemove(key, out onlineUserCache);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string RetrieveLocaleByIp(string ip = null)
        {
            if (DictHelper.RetrieveLocaleIP() == 0 || ip.IsNullOrEmpty() || _local.Any(p => p == ip)) return "本地连接";

            var url = ConfigurationManager.AppSettings["IPSvrUrl"];
            var task = _client.GetAsJsonAsync<IPLocator>($"{url}{ip}");
            task.Wait();
            return task.Result.status == "0" ? string.Join(" ", task.Result.address.SpanSplit("|").Skip(1).Take(2)) : "XX XX";
        }

        /// <summary>
        /// 
        /// </summary>
        private class IPLocator
        {
            /// <summary>
            /// 详细地址信息
            /// </summary>
            public string address { get; set; }

            /// <summary>
            /// 结果状态返回码
            /// </summary>
            public string status { get; set; }
        }
    }
}
