using Bootstrap.DataAccess;
using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace Bootstrap.Admin.Controllers
{
    public class WSController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(MessageHandler);
                response.StatusCode = System.Net.HttpStatusCode.SwitchingProtocols;
            }
            else
            {
                response.Content = new StringContent("请使用WebSocket协议请求");
            }
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async System.Threading.Tasks.Task MessageHandler(AspNetWebSocketContext arg)
        {
            WebSocket socket = arg.WebSocket;
            while (socket.State == WebSocketState.Open)
            {
                if (NotificationHelper.MessagePool.IsEmpty)
                {
                    await System.Threading.Tasks.Task.Delay(300);
                    continue;
                }
                var msg = new MessageBody();
                if (NotificationHelper.MessagePool.TryDequeue(out msg))
                {
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg.ToString()));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}