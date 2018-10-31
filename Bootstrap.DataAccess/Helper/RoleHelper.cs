using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleHelper
    {
        public const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";
        public const string RetrieveRolesByUserIdDataKey = "RoleHelper-RetrieveRolesByUserId";
        public const string RetrieveRolesByMenuIdDataKey = "RoleHelper-RetrieveRolesByMenuId";
        public const string RetrieveRolesByGroupIdDataKey = "RoleHelper-RetrieveRolesByGroupId";
        public const string RetrieveRolesByUserNameDataKey = "RoleHelper-RetrieveRolesByUserName";
        public const string RetrieveRolesByUrlDataKey = "RoleHelper-RetrieveRolesByUrl";

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRoles() => CacheManager.GetOrAdd(RetrieveRolesDataKey, key => DbAdapterManager.Create<Role>().RetrieveRoles());

        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveRolesByUserId(string userId, IEnumerable<string> roleIds)
        {
            var ret = DbAdapterManager.Create<Role>().SaveRolesByUserId(userId, roleIds);
            if (ret) CacheCleanUtility.ClearCache(userIds: new List<string>() { userId }, roleIds: roleIds);
            return ret;
        }

        /// <summary>
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByUserId(string userId) => CacheManager.GetOrAdd($"{RetrieveRolesByUserIdDataKey}-{userId}", key => DbAdapterManager.Create<Role>().RetrieveRolesByUserId(userId), RetrieveRolesByUserIdDataKey);

        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public static bool DeleteRole(IEnumerable<string> value)
        {
            var ret = DbAdapterManager.Create<Role>().DeleteRole(value);
            if (ret) CacheCleanUtility.ClearCache(roleIds: value);
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveRole(Role p)
        {
            var ret = DbAdapterManager.Create<Role>().SaveRole(p);
            if (ret) CacheCleanUtility.ClearCache(roleIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string> { p.Id });
            return ret;
        }

        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByMenuId(string menuId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByMenuIdDataKey, menuId), key => DbAdapterManager.Create<Role>().RetrieveRolesByMenuId(menuId), RetrieveRolesByMenuIdDataKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SavaRolesByMenuId(string menuId, IEnumerable<string> roleIds)
        {
            var ret = DbAdapterManager.Create<Role>().SavaRolesByMenuId(menuId, roleIds);
            if (ret) CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: new List<string>() { menuId });
            return ret;
        }

        /// <summary>
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByGroupId(string groupId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByGroupIdDataKey, groupId), key => DbAdapterManager.Create<Role>().RetrieveRolesByGroupId(groupId), RetrieveRolesByGroupIdDataKey);

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveRolesByGroupId(string groupId, IEnumerable<string> roleIds)
        {
            var ret = DbAdapterManager.Create<Role>().SaveRolesByGroupId(groupId, roleIds);
            if (ret) CacheCleanUtility.ClearCache(roleIds: roleIds, groupIds: new List<string>() { groupId });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUserNameDataKey, userName), key => DbAdapterManager.Create<Role>().RetrieveRolesByUserName(userName), RetrieveRolesByUserNameDataKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUrl(string url) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUrlDataKey, url), key => DbAdapterManager.Create<Role>().RetrieveRolesByUrl(url), RetrieveRolesByUrlDataKey);
    }
}