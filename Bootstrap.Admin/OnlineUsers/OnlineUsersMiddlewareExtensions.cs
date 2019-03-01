using Bootstrap.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

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
             await Task.Run(() =>
             {
                 var onlineUsers = context.RequestServices.GetService<IOnlineUsers>();
                 var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "::1";
                 onlineUsers.AddOrUpdate(clientIp, key =>
                 {
                     var ou = new OnlineUser();
                     ou.Ip = clientIp;
                     ou.UserName = context.User.Identity.Name;
                     ou.FirstAccessTime = DateTime.Now;
                     ou.LastAccessTime = DateTime.Now;
                     ou.Method = context.Request.Method;
                     ou.RequestUrl = context.Request.Path;
                     ou.AddRequestUrl(context.Request.Path);
                     return ou;
                 }, (key, v) =>
                 {
                     v.UserName = context.User.Identity.Name;
                     v.LastAccessTime = DateTime.Now;
                     v.Method = context.Request.Method;
                     v.RequestUrl = context.Request.Path;
                     v.AddRequestUrl(context.Request.Path);
                     return v;
                 });
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
