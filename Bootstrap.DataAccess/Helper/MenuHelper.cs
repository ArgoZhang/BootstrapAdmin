using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 菜单操作类
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// 通过指定角色ID相关菜单缓存键值
        /// </summary>
        public const string RetrieveMenusByRoleIdDataKey = "MenuHelper-RetrieveMenusByRoleId";

        /// <summary>
        /// 通过当前用户获取所有菜单数据缓存键名称 "BootstrapMenu-RetrieveMenus"
        /// </summary>
        public const string RetrieveMenusAll = DbHelper.RetrieveMenusAll;

        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Save(BootstrapMenu p)
        {
            // 不允许保存系统菜单与前台演示系统的默认菜单
            if (DictHelper.RetrieveSystemModel() && (p.Category == "0" || p.Application == "2")) return true;

            var ret = DbContextManager.Create<Menu>().Save(p);
            if (ret) CacheCleanUtility.ClearCache(menuIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string>() { p.Id });
            return ret;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Delete(IEnumerable<string> value)
        {
            if (DictHelper.RetrieveSystemModel())
            {
                // 不允许删除系统菜单与前台演示系统的默认菜单
                var systemMenus = RetrieveAllMenus("Admin").Where(m => m.Category == "0" || m.Application == "2");
                value = value.Where(v => !systemMenus.Any(m => m.Id == v));
                if (!value.Any()) return true;
            }
            var ret = DbContextManager.Create<Menu>().Delete(value);
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
        /// 通过角色获取相关菜单集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveMenusByRoleId(string roleId) => CacheManager.GetOrAdd($"{RetrieveMenusByRoleIdDataKey}-{roleId}", k => DbContextManager.Create<Menu>().RetrieveMenusByRoleId(roleId), RetrieveMenusByRoleIdDataKey);

        /// <summary>
        /// 保存指定角色的所有菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public static bool SaveMenusByRoleId(string roleId, IEnumerable<string> menuIds)
        {
            var ret = DbContextManager.Create<Menu>().SaveMenusByRoleId(roleId, menuIds);
            if (ret) CacheCleanUtility.ClearCache(menuIds: menuIds, roleIds: new List<string>() { roleId });
            return ret;
        }

        /// <summary>
        /// 获取指定用户的应用程序菜单
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="userName"></param>
        /// <param name="activeUrl"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveAppMenus(string appId, string userName, string activeUrl)
        {
            var menus = RetrieveAllMenus(userName).Where(m => m.Category == "1" && m.IsResource == 0);
            if (appId != "0") menus = menus.Where(m => m.Application == appId);
            return DbHelper.CascadeMenus(menus, activeUrl);
        }

        /// <summary>
        /// 通过当前用户名获得后台菜单，层次化后集合
        /// </summary>
        /// <param name="userName">当前登陆的用户名</param>
        /// <param name="activeUrl">当前访问菜单</param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveSystemMenus(string userName, string activeUrl = null)
        {
            var menus = RetrieveAllMenus(userName).Where(m => m.Category == "0" && m.IsResource == 0);
            return DbHelper.CascadeMenus(menus, activeUrl);
        }

        /// <summary>
        /// 通过当前用户名获得所有菜单，层次化后集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveMenus(string userName)
        {
            var menus = RetrieveAllMenus(userName);
            return DbHelper.CascadeMenus(menus);
        }

        /// <summary>
        /// 通过用户获得所有菜单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName) => CacheManager.GetOrAdd($"{RetrieveMenusAll}-{userName}", key => DbContextManager.Create<Menu>().RetrieveAllMenus(userName), RetrieveMenusAll);
    }
}
