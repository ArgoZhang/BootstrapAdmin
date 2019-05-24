using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
using Microsoft.AspNetCore.Http;
using System;
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
        public static bool Save(BootstrapMenu p)
        {
            if (DictHelper.RetrieveSystemModel())
            {
                if (p.Id.IsNullOrEmpty())
                {
                    if (p.Category == "0") p.Category = "1";
                }
                else
                {
                    if (RetrieveAllMenus("Admin").Where(m => m.Category == "0").Any(m => m.Id == p.Id))
                    {
                        return true;
                    }
                }
            }
            if (p.Id == string.Empty) p.Id = null;
            var ret = DbContextManager.Create<Menu>().Save(p);
            if (ret) CacheCleanUtility.ClearCache(menuIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string>() { p.Id });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Delete(IEnumerable<string> value)
        {
            if (DictHelper.RetrieveSystemModel())
            {
                // 不允许删除系统菜单与前台演示系统的默认菜单
                var menuNames = new string[] { "首页", "测试页面", "关于", "返回码云" };
                var systemMenus = RetrieveAllMenus("Admin").Where(m => m.Category == "0" || menuNames.Any(n => n.Equals(m.Name, StringComparison.OrdinalIgnoreCase)));
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
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<object> RetrieveMenusByRoleId(string roleId) => CacheManager.GetOrAdd($"{RetrieveMenusByRoleIdDataKey}-{roleId}", k => DbContextManager.Create<Menu>().RetrieveMenusByRoleId(roleId), RetrieveMenusByRoleIdDataKey);

        /// <summary>
        /// 
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
        /// 
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
        /// 通过当前用户名与指定菜单路径获取此菜单下所有授权按钮集合
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="url">资源按钮所属菜单</param>
        /// <param name="key">资源授权码</param>
        /// <returns></returns>
        public static bool AuthorizateButtons(HttpContext context, string url, string key)
        {
            if (context.User.IsInRole("Administrators")) return true;

            var menus = RetrieveAllMenus(context.User.Identity.Name);
            var activeMenu = menus.FirstOrDefault(m => m.Url.Equals(url, StringComparison.OrdinalIgnoreCase));
            if (activeMenu == null) return false;

            var authorKeys = menus.Where(m => m.ParentId == activeMenu.Id && m.IsResource == 2).Select(m => m.Url);
            var keys = key.SpanSplitAny(",. ;", StringSplitOptions.RemoveEmptyEntries);
            return keys.Any(m => authorKeys.Any(k => k == m));
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
