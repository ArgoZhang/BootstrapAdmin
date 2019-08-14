using System;
using System.Net.Http;

namespace Bootstrap.Admin
{
    /// <summary>
    /// Gitee HttpClient 操作类
    /// </summary>
    public class GiteeHttpClient
    {
        /// <summary>
        /// HttpClient 实例
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client"></param>
        public GiteeHttpClient(HttpClient client)
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            HttpClient = client;
        }
    }
}
