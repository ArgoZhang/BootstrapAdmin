using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// author:liuchun
    /// date:2016.10.22
    /// </summary>
    public static class GroupHelper
    {
        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroups(int id = 0) => DbAdapterManager.Create<Group>().RetrieveGroups(id);
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteGroup(IEnumerable<int> value) => DbAdapterManager.Create<Group>().DeleteGroup(value);
        /// <summary>
        /// 保存新建/更新的群组信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveGroup(Group p) => DbAdapterManager.Create<Group>().SaveGroup(p);
        /// <summary>
        /// 根据用户查询部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroupsByUserId(int userId) => DbAdapterManager.Create<Group>().RetrieveGroupsByUserId(userId);
        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool SaveGroupsByUserId(int id, IEnumerable<int> groupIds) => DbAdapterManager.Create<Group>().SaveGroupsByUserId(id, groupIds);
        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroupsByRoleId(int roleId) => DbAdapterManager.Create<Group>().RetrieveGroupsByRoleId(roleId);
        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool SaveGroupsByRoleId(int id, IEnumerable<int> groupIds) => DbAdapterManager.Create<Group>().SaveGroupsByRoleId(id, groupIds);
    }
}
