using Bootstrap.DataAccess;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Data.Common;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public static class SignalRManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static System.Threading.Tasks.Task Send(IClientProxy client, MessageBody args) => client.SendAsync("rev", args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static System.Threading.Tasks.Task SendTaskLog(IClientProxy client, string args) => client.SendAsync("taskRev", args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task Send<T>(IHubContext<T> context, Exception ex) where T : Hub
        {
            var category = "App";
            if (ex.GetType().IsSubclassOf(typeof(DbException))) category = "DB";
            var message = new MessageBody() { Category = category, Message = ex.Message };
            await Send(context.Clients.All, message);
        }
    }
}
