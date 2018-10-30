using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class GroupHelper
    {
        public const string RetrieveGroupsDataKey = "GroupHelper-RetrieveGroups";
        public const string RetrieveGroupsByUserIdDataKey = "GroupHelper-RetrieveGroupsByUserId";
        public const string RetrieveGroupsByRoleIdDataKey = "GroupHelper-RetrieveGroupsByRoleId";
        public const string RetrieveGroupsByUserNameDataKey = "GroupHelper-RetrieveGroupsByUserName";
        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroups() => CacheManager.GetOrAdd(RetrieveGroupsDataKey, key => DbAdapterManager.Create<Group>().RetrieveGroups());
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteGroup(IEnumerable<string> value) => DbAdapterManager.Create<Group>().DeleteGroup(value);
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
        public static IEnumerable<Group> RetrieveGroupsByUserId(string userId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByUserIdDataKey, userId), k => DbAdapterManager.Create<Group>().RetrieveGroupsByUserId(userId), RetrieveGroupsByUserIdDataKey);

        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool SaveGroupsByUserId(string userId, IEnumerable<string> groupIds) => DbAdapterManager.Create<Group>().SaveGroupsByUserId(userId, groupIds);
        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroupsByRoleId(string roleId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByRoleIdDataKey, roleId), key => DbAdapterManager.Create<Group>().RetrieveGroupsByRoleId(roleId), RetrieveGroupsByRoleIdDataKey);
        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool SaveGroupsByRoleId(string roleId, IEnumerable<string> groupIds) => DbAdapterManager.Create<Group>().SaveGroupsByRoleId(roleId, groupIds);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveGroupsByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByUserNameDataKey, userName), r => DbAdapterManager.Create<Group>().RetrieveGroupsByUserName(userName), RetrieveGroupsByUserNameDataKey);
    }
}
