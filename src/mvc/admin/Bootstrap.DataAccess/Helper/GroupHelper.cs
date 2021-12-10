using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class GroupHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveGroupsDataKey = "GroupHelper-RetrieveGroups";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveGroupsByUserIdDataKey = "GroupHelper-RetrieveGroupsByUserId";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveGroupsByRoleIdDataKey = "GroupHelper-RetrieveGroupsByRoleId";
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveGroupsByUserNameDataKey = DbHelper.RetrieveGroupsByUserNameDataKey;

        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Group> Retrieves() => CacheManager.GetOrAdd(RetrieveGroupsDataKey, key => DbContextManager.Create<Group>()?.Retrieves()) ?? new Group[0];

        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="values"></param>
        public static bool Delete(IEnumerable<string> values)
        {
            var ret = DbContextManager.Create<Group>()?.Delete(values) ?? false;
            if (ret) CacheCleanUtility.ClearCache(groupIds: values);
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的群组信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Save(Group p)
        {
            var ret = DbContextManager.Create<Group>()?.Save(p) ?? false;
            if (ret) CacheCleanUtility.ClearCache(groupIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string>() { p.Id });
            return ret;
        }
        /// <summary>
        /// 根据用户查询部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrievesByUserId(string userId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByUserIdDataKey, userId), k => DbContextManager.Create<Group>()?.RetrievesByUserId(userId), RetrieveGroupsByUserIdDataKey) ?? new Group[0];

        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool SaveByUserId(string userId, IEnumerable<string> groupIds)
        {
            var ret = DbContextManager.Create<Group>()?.SaveByUserId(userId, groupIds) ?? false;
            if (ret) CacheCleanUtility.ClearCache(groupIds: groupIds, userIds: new List<string>() { userId });
            return ret;
        }

        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrievesByRoleId(string roleId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByRoleIdDataKey, roleId), key => DbContextManager.Create<Group>()?.RetrievesByRoleId(roleId), RetrieveGroupsByRoleIdDataKey) ?? new Group[0];

        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool SaveByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            var ret = DbContextManager.Create<Group>()?.SaveByRoleId(roleId, groupIds) ?? false;
            if (ret) CacheCleanUtility.ClearCache(groupIds: groupIds, roleIds: new List<string>() { roleId });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapGroup> RetrievesByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByUserNameDataKey, userName), r => DbContextManager.Create<Group>()?.RetrievesByUserName(userName), RetrieveGroupsByUserNameDataKey) ?? new BootstrapGroup[0];
    }
}
