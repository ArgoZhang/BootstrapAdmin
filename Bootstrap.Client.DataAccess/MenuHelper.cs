using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Configuration;
using System.Collections.Generic;
using System.Linq;

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
        public const string RetrieveMenusAll = "BootstrapMenu-RetrieveMenus";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="activeUrl"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveAppMenus(string userName, string activeUrl)
        {
            var menus = RetrieveAllMenus(userName).Where(m => m.Category == "1" && m.IsResource == 0 && m.ApplicationCode == ConfigurationManager.AppSettings["AppId"]);
            DbHelper.ActiveMenu(null, menus, activeUrl);
            var root = menus.Where(m => m.ParentId == "0").OrderBy(m => m.ApplicationCode).ThenBy(m => m.Order);
            DbHelper.CascadeMenus(menus, root);
            return root;
        }
        /// <summary>
        /// 通过用户获得所有菜单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName) => CacheManager.GetOrAdd($"{RetrieveMenusAll}-{userName}", key => DbHelper.RetrieveAllMenus(userName), RetrieveMenusAll);
    }
}
