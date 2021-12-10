using Longbow.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// Bootstrap TraceHttpClient 操作类
    /// </summary>
    public class TraceHttpClient
    {
        HttpClient _client;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client"></param>
        public TraceHttpClient(HttpClient client) => _client = client;

        /// <summary>
        /// 提交数据到后台访问网页接口
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        public void Post(HttpContext context, OnlineUser user)
        {
            // 调用 后台跟踪接口
            // http://localhost:50852/api/Traces
            user.RequestUrl = context.Request.AbsoluteUrl();

            try
            {
                var t = _client.PostAsJsonAsync("", user, context.RequestAborted);
                t.Wait(2000);
            }
            catch (Exception ex)
            {
                ex.Log(new NameValueCollection() { ["RequestUrl"] = _client.BaseAddress!.AbsoluteUri });
            }
        }
    }
}
