using Bootstrap.Security;
using Longbow;
using Longbow.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Bootstrap.Client.DataAccess
{
    internal static class BAHelper
    {
        private readonly static LgbHttpClient _client = new LgbHttpClient(new HttpClient() { BaseAddress = new Uri($"{ConfigurationManager.AppSettings["AuthHost"]}/api/Interface/") });

        private static T ExecuteRemoteAction<T>(string actionName, object data = null)
        {
            var task = _client.PostAsJsonAsync<T>(actionName, data);
            task.Wait();
            return task.Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveAppMenus(object args) => ExecuteRemoteAction<IEnumerable<BootstrapMenu>>("RetrieveAppMenus", args);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUrl(string url) => ExecuteRemoteAction<IEnumerable<string>>("RetrieveRolesByUrl", url);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUserName(string userName) => ExecuteRemoteAction<IEnumerable<string>>("RetrieveRolesByUserName", userName);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveDicts() => ExecuteRemoteAction<IEnumerable<BootstrapDict>>("RetrieveDicts");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser RetrieveUserByUserName(string userName) => ExecuteRemoteAction<BootstrapUser>("RetrieveUserByUserName", userName);
    }
}
