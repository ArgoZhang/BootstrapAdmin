using Bootstrap.Admin;
using Bootstrap.DataAccess;
using Longbow.Cache;
using Longbow.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class OnlineUsersMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOnlineUsers(this IApplicationBuilder builder) => builder.UseWhen(context => context.Filter(), app => app.Use(async (context, next) =>
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                var user = UserHelper.RetrieveUserByUserName(context.User.Identity.Name);
                if (user == null) return;

                var onlineUserSvr = context.RequestServices.GetRequiredService<IOnlineUsers>();
                var proxy = new Func<AutoExpireCacheEntry<OnlineUser>, Action, AutoExpireCacheEntry<OnlineUser>>((c, action) =>
                {
                    var v = c.Value;
                    v.UserName = user.UserName;
                    v.DisplayName = user.DisplayName;
                    v.LastAccessTime = DateTime.Now;
                    v.Method = context.Request.Method;
                    v.RequestUrl = context.Request.Path;
                    v.AddRequestUrl(context.Request.Path);
                    action?.Invoke();
                    TraceHelper.Save(new Trace { Ip = v.Ip, RequestUrl = v.RequestUrl, LogTime = v.LastAccessTime, City = v.Location, Browser = v.Browser, OS = v.OS, UserName = v.UserName });
                    return c;
                });
                onlineUserSvr.AddOrUpdate(context.Connection.Id ?? "", key =>
                {
                    var agent = new UserAgent(context.Request.Headers["User-Agent"]);
                    var v = new OnlineUser();
                    v.ConnectionId = key;
                    v.Ip = (context.Connection.RemoteIpAddress ?? IPAddress.IPv6Loopback).ToString();
                    v.Location = onlineUserSvr.RetrieveLocaleByIp(v.Ip);
                    v.Browser = $"{agent.Browser.Name} {agent.Browser.Version}";
                    v.OS = $"{agent.OS.Name} {agent.OS.Version}";
                    v.FirstAccessTime = DateTime.Now;
                    return proxy(new AutoExpireCacheEntry<OnlineUser>(v, 1000 * 60, __ => onlineUserSvr.TryRemove(key, out _)), null);
                }, (key, v) => proxy(v, () => v.Reset()));
            });
            await next();
        }));

        private static bool Filter(this HttpContext context)
        {
            var url = context.Request.Path;
            return !new string[] { "/api", "/NotiHub", "/swagger" }.Any(r => url.StartsWithSegments(r, StringComparison.OrdinalIgnoreCase));
        }
    }
}
