using Longbow.Cache;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveRolesByUserIdDataKey = "RoleHelper-RetrieveRolesByUserId";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveRolesByMenuIdDataKey = "RoleHelper-RetrieveRolesByMenuId";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveRolesByGroupIdDataKey = "RoleHelper-RetrieveRolesByGroupId";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveRolesByUserNameDataKey = "RoleHelper-RetrieveRolesByUserName";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveRolesByUrlDataKey = "RoleHelper-RetrieveRolesByUrl";

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> Retrieves() => CacheManager.GetOrAdd(RetrieveRolesDataKey, key => DbContextManager.Create<Role>().Retrieves());

        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveByUserId(string userId, IEnumerable<string> roleIds)
        {
            var ret = DbContextManager.Create<Role>().SaveByUserId(userId, roleIds);
            if (ret) CacheCleanUtility.ClearCache(userIds: new List<string>() { userId }, roleIds: roleIds);
            return ret;
        }

        /// <summary>
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> RetrievesByUserId(string userId) => CacheManager.GetOrAdd($"{RetrieveRolesByUserIdDataKey}-{userId}", key => DbContextManager.Create<Role>().RetrievesByUserId(userId), RetrieveRolesByUserIdDataKey);

        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public static bool Delete(IEnumerable<string> value)
        {
            var roles = new string[] { "Administrators", "Default" };
            var rs = Retrieves().Where(r => roles.Any(rl => rl.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase)));
            value = value.Where(v => !rs.Any(r => r.Id == v));
            if (!value.Any()) return true;
            var ret = DbContextManager.Create<Role>().Delete(value);
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
            var roles = new string[] { "Administrators", "Default" };
            var rs = Retrieves().Where(r => roles.Any(rl => rl.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase)));
            if (rs.Any(r => r.Id == p.Id)) return true;
            if (p.Id == string.Empty) p.Id = null;
            var ret = DbContextManager.Create<Role>().Save(p);
            if (ret) CacheCleanUtility.ClearCache(roleIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string> { p.Id });
            return ret;
        }

        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrievesByMenuId(string menuId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByMenuIdDataKey, menuId), key => DbContextManager.Create<Role>().RetrievesByMenuId(menuId), RetrieveRolesByMenuIdDataKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SavaByMenuId(string menuId, IEnumerable<string> roleIds)
        {
            var ret = DbContextManager.Create<Role>().SavaByMenuId(menuId, roleIds);
            if (ret) CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: new List<string>() { menuId });
            return ret;
        }

        /// <summary>
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrievesByGroupId(string groupId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByGroupIdDataKey, groupId), key => DbContextManager.Create<Role>().RetrievesByGroupId(groupId), RetrieveRolesByGroupIdDataKey);

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveByGroupId(string groupId, IEnumerable<string> roleIds)
        {
            var ret = DbContextManager.Create<Role>().SaveByGroupId(groupId, roleIds);
            if (ret) CacheCleanUtility.ClearCache(roleIds: roleIds, groupIds: new List<string>() { groupId });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUserNameDataKey, userName), key => DbContextManager.Create<Role>().RetrievesByUserName(userName), RetrieveRolesByUserNameDataKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUrl(string url) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUrlDataKey, url), key => DbContextManager.Create<Role>().RetrievesByUrl(url), RetrieveRolesByUrlDataKey);
    }
}
