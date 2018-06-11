using Longbow.Configuration.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bootstrap.Admin
{
    public class WebSocketHandlerMiddleware : LgbMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public WebSocketHandlerMiddleware(RequestDelegate next) : base(next)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest || !context.User.Identity.IsAuthenticated) return;
            using (var socket = await context.WebSockets.AcceptWebSocketAsync())
            {
                while (socket.State == WebSocketState.Open)
                {
                    await Task.Delay(60000);
                    var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(DateTimeOffset.Now.ToString()));
                    await socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

    public static class WebSocketExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void UseWebSocketHandler(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Map("/Foo", builder => builder.UseMiddleware<WebSocketHandlerMiddleware>());
        }
    }
}