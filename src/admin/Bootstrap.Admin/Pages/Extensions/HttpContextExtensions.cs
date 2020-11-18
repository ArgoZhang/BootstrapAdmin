using Bootstrap.DataAccess;
using Longbow.Cache;
using Longbow.Web;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// HttpContextExtensions 扩展方法
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 保存访问日志方法
        /// </summary>
        public static void SaveOnlineUser(this HttpContext context, string url)
        {
            var onlineUserSvr = context.RequestServices.GetRequiredService<IOnlineUsers>();
            var locator = context.RequestServices.GetRequiredService<IIPLocatorProvider>();
            var proxy = new Func<AutoExpireCacheEntry<OnlineUser>, Action?, AutoExpireCacheEntry<OnlineUser>>((c, action) =>
            {
                var v = c.Value;
                v.LastAccessTime = DateTime.Now;
                v.Method = context.Request.Method;
                v.RequestUrl = url;
                v.AddRequestUrl(url);
                action?.Invoke();
                TraceHelper.Save(context, v);
                return c;
            });
            onlineUserSvr.AddOrUpdate(context.Connection.Id ?? "", key =>
            {
                var agent = context.Request.Headers["User-Agent"];
                var userAgent = string.IsNullOrEmpty(agent) ? null : new UserAgent(agent);
                var v = new OnlineUser
                {
                    UserAgent = agent,
                    ConnectionId = key,
                    Ip = context.Connection.RemoteIpAddress?.ToIPv4String() ?? string.Empty,
                    Browser = userAgent == null ? "Unknown" : $"{userAgent.Browser?.Name} {userAgent.Browser?.Version}",
                    OS = userAgent == null ? "Unknown" : $"{userAgent.OS?.Name} {userAgent.OS?.Version}",
                    FirstAccessTime = DateTime.Now,
                    Referer = context.Request.Headers["Referer"]
                };
                v.Location = locator?.Locate(v.Ip) ?? "";
                return proxy(new AutoExpireCacheEntry<OnlineUser>(v, 1000 * 60, __ => onlineUserSvr.TryRemove(key, out _)), null);
            }, (key, v) => proxy(v, () => v.Reset()));
        }
    }
}
