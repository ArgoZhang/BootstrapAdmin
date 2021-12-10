using Longbow.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 访问网页跟踪帮助类
    /// </summary>
    public static class TraceHelper
    {
        /// <summary>
        /// 保存访问历史记录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        public static void Save(HttpContext context, OnlineUser user)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var client = context.RequestServices.GetRequiredService<TraceHttpClient>();
                client.Post(context, user);
            }
        }
    }
}
