using Bootstrap.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    internal class DefaultOnlineUsers : IOnlineUsers
    {
        private ConcurrentDictionary<string, OnlineUserCache> _onlineUsers = new ConcurrentDictionary<string, OnlineUserCache>();
        private ConcurrentDictionary<string, string> _ipLocator = new ConcurrentDictionary<string, string>();
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
            if (string.IsNullOrEmpty(DictHelper.RetrieveLocaleIPSvr()) || ip.IsNullOrEmpty() || _local.Any(p => p == ip)) return "本地连接";

            return _ipLocator.GetOrAdd(ip, key =>
            {
                var ipSvr = DictHelper.RetrieveLocaleIPSvr();
                var url = $"{DictHelper.RetrieveLocaleIPSvrUrl(ipSvr)}{ip}";
                var task = ipSvr == "BaiDuIPSvr" ? RetrieveLocator<BaiDuIPLocator>(url) : RetrieveLocator<JuheIPLocator>(url);
                task.Wait();
                return task.Result;
            });
        }

        private async Task<string> RetrieveLocator<T>(string url)
        {
            var result = await _client.GetAsJsonAsync<T>(url);
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private class BaiDuIPLocator
        {
            /// <summary>
            /// 详细地址信息
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// 结果状态返回码
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Status == "0" ? string.Join(" ", Address.SpanSplit("|").Skip(1).Take(2)) : "XX XX";
            }
        }

        private class JuheIPLocator
        {
            /// <summary>
            /// 
            /// </summary>
            public string ResultCode { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Reason { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public JuheIPLocatorResult Result { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public int Error_Code { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Error_Code != 0 ? "XX XX" : Result.ToString();
            }
        }

        private class JuheIPLocatorResult
        {
            /// <summary>
            /// 
            /// </summary>
            public string Country { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Province { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Isp { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Country != "中国" ? $"{Country} {Province} {Isp}" : $"{Province} {City} {Isp}";
            }
        }
    }
}
