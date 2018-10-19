using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleHelper
    {
        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRoles(int id = 0) => DbAdapterManager.Create<Role>().RetrieveRoles(id);
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveRolesByUserId(int id, IEnumerable<int> roleIds) => DbAdapterManager.Create<Role>().SaveRolesByUserId(id, roleIds);
        /// <summary>
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByUserId(int userId) => DbAdapterManager.Create<Role>().RetrieveRolesByUserId(userId);
        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public static bool DeleteRole(IEnumerable<int> value) => DbAdapterManager.Create<Role>().DeleteRole(value);
        /// <summary>
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveRole(Role p) => DbAdapterManager.Create<Role>().SaveRole(p);
        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByMenuId(int menuId) => DbAdapterManager.Create<Role>().RetrieveRolesByMenuId(menuId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SavaRolesByMenuId(int id, IEnumerable<int> roleIds) => DbAdapterManager.Create<Role>().SavaRolesByMenuId(id, roleIds);
        /// <summary>
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByGroupId(int groupId) => DbAdapterManager.Create<Role>().RetrieveRolesByGroupId(groupId);
        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveRolesByGroupId(int id, IEnumerable<int> roleIds) => DbAdapterManager.Create<Role>().SaveRolesByGroupId(id, roleIds);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUserName(string userName) => DbAdapterManager.Create<Role>().RetrieveRolesByUserName(userName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveRolesByUrl(string url) => DbAdapterManager.Create<Role>().RetrieveRolesByUrl(url);
    }
}