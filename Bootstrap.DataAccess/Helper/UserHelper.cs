using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户表相关操作类
    /// </summary>
    public static class UserHelper
    {
        public const string RetrieveUsersDataKey = "BootstrapUser-RetrieveUsers";
        public const string RetrieveUsersByRoleIdDataKey = "BootstrapUser-RetrieveUsersByRoleId";
        public const string RetrieveUsersByGroupIdDataKey = "BootstrapUser-RetrieveUsersByGroupId";
        public const string RetrieveNewUsersDataKey = "UserHelper-RetrieveNewUsers";
        public const string RetrieveUsersByNameDataKey = "BootstrapUser-RetrieveUsersByName";
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsers() => CacheManager.GetOrAdd(RetrieveUsersDataKey, key => DbAdapterManager.Create<User>().RetrieveUsers());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Authenticate(string userName, string password) => DbAdapterManager.Create<User>().Authenticate(userName, password);
        /// <summary>
        /// 查询所有的新注册用户
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveNewUsers() => CacheManager.GetOrAdd(RetrieveNewUsersDataKey, key => DbAdapterManager.Create<User>().RetrieveNewUsers());
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="value"></param>
        public static bool DeleteUser(IEnumerable<int> value) => DbAdapterManager.Create<User>().DeleteUser(value);
        /// <summary>
        /// 保存新建
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveUser(User p) => DbAdapterManager.Create<User>().SaveUser(p);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool UpdateUser(int id, string password, string displayName) => DbAdapterManager.Create<User>().UpdateUser(id, password, displayName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        public static bool ApproveUser(int id, string approvedBy) => DbAdapterManager.Create<User>().ApproveUser(id, approvedBy);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public static bool ChangePassword(string userName, string password, string newPass) => DbAdapterManager.Create<User>().ChangePassword(userName, password, newPass);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rejectBy"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static bool RejectUser(int id, string rejectBy) => DbAdapterManager.Create<User>().RejectUser(id, rejectBy);
        /// <summary>
        /// 通过roleId获取所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsersByRoleId(int roleId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByRoleIdDataKey, roleId), k => DbAdapterManager.Create<User>().RetrieveUsersByRoleId(roleId), RetrieveUsersByRoleIdDataKey);
        /// <summary>
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveUsersByRoleId(int id, IEnumerable<int> userIds) => DbAdapterManager.Create<User>().SaveUsersByRoleId(id, userIds);
        /// <summary>
        /// 通过groupId获取所有用户
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsersByGroupId(int groupId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByGroupIdDataKey, groupId), k => DbAdapterManager.Create<User>().RetrieveUsersByGroupId(groupId), RetrieveUsersByRoleIdDataKey);
        /// <summary>
        /// 通过部门ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveUsersByGroupId(int id, IEnumerable<int> userIds) => DbAdapterManager.Create<User>().SaveUsersByGroupId(id, userIds);
        /// 根据用户名修改用户头像
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public static bool SaveUserIconByName(string userName, string iconName) => DbAdapterManager.Create<User>().SaveUserIconByName(userName, iconName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool SaveDisplayName(string userName, string displayName) => DbAdapterManager.Create<User>().SaveDisplayName(userName, displayName);
        /// <summary>
        /// 根据用户名更改用户皮肤
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public static bool SaveUserCssByName(string userName, string cssName) => DbAdapterManager.Create<User>().SaveUserCssByName(userName, cssName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser RetrieveUserByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName), k => DbAdapterManager.Create<User>().RetrieveUserByUserName(userName), RetrieveUsersByNameDataKey);
    }
}
