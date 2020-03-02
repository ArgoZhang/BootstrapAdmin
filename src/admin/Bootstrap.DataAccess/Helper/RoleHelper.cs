using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 角色操作帮助类
    /// </summary>
    public static class RoleHelper
    {
        /// <summary>
        /// 获取所有角色数据缓存键值 RoleHelper-RetrieveRoles
        /// </summary>
        public const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";
        /// <summary>
        /// 通过用户 ID 获取相关角色集合键值 RoleHelper-RetrieveRolesByUserId
        /// </summary>
        public const string RetrieveRolesByUserIdDataKey = "RoleHelper-RetrieveRolesByUserId";
        /// <summary>
        /// 通过菜单 ID 获得相关角色集合键值 RoleHelper-RetrieveRolesByMenuId
        /// </summary>
        public const string RetrieveRolesByMenuIdDataKey = "RoleHelper-RetrieveRolesByMenuId";
        /// <summary>
        /// 通过部门 ID 获得相关角色集合键值 RoleHelper-RetrieveRolesByGroupId
        /// </summary>
        public const string RetrieveRolesByGroupIdDataKey = "RoleHelper-RetrieveRolesByGroupId";

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> Retrieves() => CacheManager.GetOrAdd(RetrieveRolesDataKey, key => DbContextManager.Create<Role>()?.Retrieves()) ?? new Role[0];

        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveByUserId(string userId, IEnumerable<string> roleIds)
        {
            // 演示模式时禁止修改 Admin 对 Administrators 角色的移除操作
            var ret = false;
            if (DictHelper.RetrieveSystemModel())
            {
                var users = new string[] { "Admin", "User" };
                var userIds = UserHelper.Retrieves().Where(u => users.Any(usr => usr.Equals(u.UserName, StringComparison.OrdinalIgnoreCase))).Select(u => u.Id);
                if (userIds.Any(u => (u ?? string.Empty).Equals(userId, StringComparison.OrdinalIgnoreCase))) ret = true;
            }
            if (ret) return ret;

            ret = DbContextManager.Create<Role>()?.SaveByUserId(userId, roleIds) ?? false;
            if (ret) CacheCleanUtility.ClearCache(userIds: new List<string>() { userId }, roleIds: roleIds);
            return ret;
        }

        /// <summary>
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> RetrievesByUserId(string userId) => CacheManager.GetOrAdd($"{RetrieveRolesByUserIdDataKey}-{userId}", key => DbContextManager.Create<Role>()?.RetrievesByUserId(userId), RetrieveRolesByUserIdDataKey) ?? new Role[0];

        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public static bool Delete(IEnumerable<string> value)
        {
            // 内置两个角色禁止修改
            var roles = new string[] { "Administrators", "Default" };
            var rs = Retrieves().Where(r => roles.Any(rl => rl.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase)));
            value = value.Where(v => !rs.Any(r => r.Id == v));
            if (!value.Any()) return true;

            var ret = DbContextManager.Create<Role>()?.Delete(value) ?? false;
            if (ret) CacheCleanUtility.ClearCache(roleIds: value);
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Save(Role p)
        {
            // 内置两个角色禁止修改
            var roles = Retrieves().Where(r => new string[] { "Administrators", "Default" }.Any(s => s.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase))).Select(r => r.Id ?? "");
            if (!string.IsNullOrEmpty(p.Id) && roles.Any(r => r.Equals(p.Id, StringComparison.OrdinalIgnoreCase))) return true;

            var ret = DbContextManager.Create<Role>()?.Save(p) ?? false;
            if (ret) CacheCleanUtility.ClearCache(roleIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string> { p.Id });
            return ret;
        }

        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrievesByMenuId(string menuId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByMenuIdDataKey, menuId), key => DbContextManager.Create<Role>()?.RetrievesByMenuId(menuId), RetrieveRolesByMenuIdDataKey) ?? new Role[0];

        /// <summary>
        /// 通过指定菜单ID保存角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SavaByMenuId(string menuId, IEnumerable<string> roleIds)
        {
            var ret = DbContextManager.Create<Role>()?.SavaByMenuId(menuId, roleIds) ?? false;
            if (ret) CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: new List<string>() { menuId });
            return ret;
        }

        /// <summary>
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrievesByGroupId(string groupId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByGroupIdDataKey, groupId), key => DbContextManager.Create<Role>()?.RetrievesByGroupId(groupId), RetrieveRolesByGroupIdDataKey) ?? new Role[0];

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveByGroupId(string groupId, IEnumerable<string> roleIds)
        {
            var ret = DbContextManager.Create<Role>()?.SaveByGroupId(groupId, roleIds) ?? false;
            if (ret) CacheCleanUtility.ClearCache(roleIds: roleIds, groupIds: new List<string>() { groupId });
            return ret;
        }

        /// <summary>
        /// 通过用户名获取授权角色集合
        /// </summary>
        /// <param name="userName">指定用户名</param>
        /// <returns>角色名称集合</returns>
        public static IEnumerable<string> RetrievesByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", DbHelper.RetrieveRolesByUserNameDataKey, userName), key => DbContextManager.Create<Role>()?.RetrievesByUserName(userName), DbHelper.RetrieveRolesByUserNameDataKey) ?? new string[0];

        /// <summary>
        /// 通过指定 Url 地址获得授权角色集合
        /// </summary>
        /// <param name="url">请求 Url 地址</param>
        /// <param name="appId">应用程序Id</param>
        /// <returns>角色名称集合</returns>
        public static IEnumerable<string> RetrievesByUrl(string url, string appId) => CacheManager.GetOrAdd(string.Format("{0}-{1}-{2}", DbHelper.RetrieveRolesByUrlDataKey, url, appId), key => DbContextManager.Create<Role>()?.RetrievesByUrl(url, appId), DbHelper.RetrieveRolesByUrlDataKey) ?? new string[0];
    }
}
