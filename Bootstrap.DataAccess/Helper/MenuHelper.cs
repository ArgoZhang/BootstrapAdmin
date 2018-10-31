using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveMenusByRoleIdDataKey = "MenuHelper-RetrieveMenusByRoleId";

        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveMenusAll = "BootstrapMenu-RetrieveMenus";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveMenu(BootstrapMenu p)
        {
            var ret = DbAdapterManager.Create<Menu>().SaveMenu(p);
            if (ret) CacheCleanUtility.ClearCache(menuIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string>() { p.Id });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool DeleteMenu(IEnumerable<string> value)
        {
            var ret = DbAdapterManager.Create<Menu>().DeleteMenu(value);
            if (ret) CacheCleanUtility.ClearCache(menuIds: value);
            return ret;
        }

        /// <summary>
        /// 通过用户名获得所有菜单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveMenusByUserName(string userName) => RetrieveAllMenus(userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<object> RetrieveMenusByRoleId(string roleId) => CacheManager.GetOrAdd($"{RetrieveMenusByRoleIdDataKey}-{roleId}", k => DbAdapterManager.Create<Menu>().RetrieveMenusByRoleId(roleId), RetrieveMenusByRoleIdDataKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public static bool SaveMenusByRoleId(string roleId, IEnumerable<string> menuIds)
        {
            var ret = DbAdapterManager.Create<Menu>().SaveMenusByRoleId(roleId, menuIds);
            if (ret) CacheCleanUtility.ClearCache(menuIds: menuIds, roleIds: new List<string>() { roleId });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="activeUrl"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveAppMenus(string appId, string userName, string activeUrl)
        {
            var menus = RetrieveAllMenus(userName).Where(m => m.Category == "1" && m.IsResource == 0);
            if (appId != "0") menus = menus.Where(m => m.ApplicationCode == appId);
            DbHelper.ActiveMenu(null, menus, activeUrl);
            var root = menus.Where(m => m.ParentId == "0").OrderBy(m => m.ApplicationCode).ThenBy(m => m.Order);
            DbHelper.CascadeMenus(menus, root);
            return root;
        }

        /// <summary>
        /// 通过当前用户名获得后台菜单，层次化后集合
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userName">当前登陆的用户名</param>
        /// <param name="activeUrl">当前访问菜单</param>
        /// <param name="connName">连接字符串名称，默认为ba</param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveSystemMenus(string userName, string activeUrl = null)
        {
            var menus = RetrieveAllMenus(userName).Where(m => m.Category == "0" && m.IsResource == 0);
            DbHelper.ActiveMenu(null, menus, activeUrl);
            var root = menus.Where(m => m.ParentId == "0").OrderBy(m => m.ApplicationCode).ThenBy(m => m.Order);
            DbHelper.CascadeMenus(menus, root);
            return root;
        }

        /// <summary>
        /// 通过当前用户名获得所有菜单，层次化后集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<object> RetrieveMenus(string userName)
        {
            var menus = RetrieveAllMenus(userName);
            var root = menus.Where(m => m.ParentId == "0").OrderBy(m => m.ApplicationCode).ThenBy(m => m.Order);
            DbHelper.CascadeMenus(menus, root);
            return root;
        }

        /// <summary>
        /// 通过用户获得所有菜单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName) => CacheManager.GetOrAdd($"{RetrieveMenusAll}-{userName}", key => DbAdapterManager.Create<Menu>().RetrieveAllMenus(userName), RetrieveMenusAll);
    }
}
