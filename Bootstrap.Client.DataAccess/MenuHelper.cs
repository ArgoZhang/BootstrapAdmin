using Bootstrap.Security;
using Longbow.Configuration;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveAppMenus(string userName, string url) => BAHelper.RetrieveAppMenus(ConfigurationManager.AppSettings["AppId"], userName, url);
    }
}
