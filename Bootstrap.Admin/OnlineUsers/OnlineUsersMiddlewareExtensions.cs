using Bootstrap.Admin;
using Bootstrap.DataAccess;
using Longbow.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
                 var onlineUserSvr = context.RequestServices.GetRequiredService<IOnlineUsers>();
                 var proxy = new Func<OnlineUserCache, Action, OnlineUserCache>((c, action) =>
                 {
                     var v = c.User;
                     v.UserName = context.User.Identity.Name;
                     if (!v.UserName.IsNullOrEmpty()) v.DisplayName = UserHelper.RetrieveUserByUserName(v.UserName).DisplayName;
                     v.LastAccessTime = DateTime.Now;
                     v.Method = context.Request.Method;
                     v.RequestUrl = context.Request.Path;
                     v.AddRequestUrl(context.Request.Path);
                     action?.Invoke();
                     return c;
                 });
                 onlineUserSvr.AddOrUpdate(context.Connection.Id ?? "", key =>
                 {
                     var agent = new UserAgent(context.Request.Headers["User-Agent"]);
                     var v = new OnlineUser();
                     v.ConnectionId = key;
                     v.Ip = context.Connection.RemoteIpAddress?.ToString();
                     v.Location = onlineUserSvr.RetrieveLocaleByIp(v.Ip);
                     v.Browser = $"{agent.Browser.Name} {agent.Browser.Version}";
                     v.OS = $"{agent.OS.Name} {agent.OS.Version}";
                     v.FirstAccessTime = DateTime.Now;
                     return proxy(new OnlineUserCache(v, () => onlineUserSvr.TryRemove(key, out _)), null);
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
