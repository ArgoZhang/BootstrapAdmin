using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;
using System.Linq;

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
        public const string RetrieveGroupsByUserNameDataKey = "BootstrapAdminGroupMiddleware-RetrieveGroupsByUserName";
        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroups(int id = 0)
        {
            var ret = CacheManager.GetOrAdd(RetrieveGroupsDataKey, key => DbAdapterManager.Create<Group>().RetrieveGroups(id));
            return id == 0 ? ret : ret.Where(t => id == t.Id);
        }

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
        public static IEnumerable<Group> RetrieveGroupsByUserId(int userId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByUserIdDataKey, userId), k => DbAdapterManager.Create<Group>().RetrieveGroupsByUserId(userId), RetrieveGroupsByUserIdDataKey);

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
        public static IEnumerable<Group> RetrieveGroupsByRoleId(int roleId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByRoleIdDataKey, roleId), key => DbAdapterManager.Create<Group>().RetrieveGroupsByRoleId(roleId), RetrieveGroupsByRoleIdDataKey);
        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static bool SaveGroupsByRoleId(int id, IEnumerable<int> groupIds) => DbAdapterManager.Create<Group>().SaveGroupsByRoleId(id, groupIds);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveGroupsByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByUserNameDataKey, userName), r => DbAdapterManager.Create<Group>().RetrieveGroupsByUserName(userName), RetrieveGroupsByUserNameDataKey);
    }
}
