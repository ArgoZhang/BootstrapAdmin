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
        public const string RetrieveUsersDataKey = "UserHelper-RetrieveUsers";
        public const string RetrieveUsersByRoleIdDataKey = "UserHelper-RetrieveUsersByRoleId";
        public const string RetrieveUsersByGroupIdDataKey = "UserHelper-RetrieveUsersByGroupId";
        public const string RetrieveNewUsersDataKey = "UserHelper-RetrieveNewUsers";
        public const string RetrieveUsersByNameDataKey = "BootstrapUser-RetrieveUsersByName";

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<User> Retrieves() => CacheManager.GetOrAdd(RetrieveUsersDataKey, key => DbContextManager.Create<User>().Retrieves());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Authenticate(string userName, string password) => DbContextManager.Create<User>().Authenticate(userName, password);

        /// <summary>
        /// 查询所有的新注册用户
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveNewUsers() => CacheManager.GetOrAdd(RetrieveNewUsersDataKey, key => DbContextManager.Create<User>().RetrieveNewUsers());

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="value"></param>
        public static bool Delete(IEnumerable<string> value)
        {
            var ret = DbContextManager.Create<User>().Delete(value);
            if (ret) CacheCleanUtility.ClearCache(userIds: value);
            return ret;
        }

        /// <summary>
        /// 保存新建
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Save(User p)
        {
            var ret = DbContextManager.Create<User>().Save(p);
            if (ret) CacheCleanUtility.ClearCache(userIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string>() { p.Id });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool Update(string id, string password, string displayName)
        {
            var ret = DbContextManager.Create<User>().Update(id, password, displayName);
            if (ret) CacheCleanUtility.ClearCache(userIds: string.IsNullOrEmpty(id) ? new List<string>() : new List<string>() { id });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        public static bool Approve(string id, string approvedBy)
        {
            var ret = DbContextManager.Create<User>().Approve(id, approvedBy);
            if (ret) CacheCleanUtility.ClearCache(userIds: new List<string>() { id });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public static bool ChangePassword(string userName, string password, string newPass) => DbContextManager.Create<User>().ChangePassword(userName, password, newPass);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="displayName"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static bool ForgotPassword(string userName, string displayName, string desc)
        {
            // UNDONE 忘记密码涉及到安全问题，防止用户恶意重置其他用户，待定
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rejectBy"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static bool Reject(string id, string rejectBy)
        {
            var ret = DbContextManager.Create<User>().Reject(id, rejectBy);
            if (ret) CacheCleanUtility.ClearCache(userIds: new List<string>() { id });
            return ret;
        }

        /// <summary>
        /// 通过roleId获取所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrievesByRoleId(string roleId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByRoleIdDataKey, roleId), k => DbContextManager.Create<User>().RetrievesByRoleId(roleId), RetrieveUsersByRoleIdDataKey);

        /// <summary>
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveByRoleId(string roleId, IEnumerable<string> userIds)
        {
            var ret = DbContextManager.Create<User>().SaveByRoleId(roleId, userIds);
            if (ret) CacheCleanUtility.ClearCache(userIds: userIds, roleIds: new List<string>() { roleId });
            return ret;
        }

        /// <summary>
        /// 通过groupId获取所有用户
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrievesByGroupId(string groupId) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByGroupIdDataKey, groupId), k => DbContextManager.Create<User>().RetrievesByGroupId(groupId), RetrieveUsersByRoleIdDataKey);

        /// <summary>
        /// 通过部门ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="groupId">GroupID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveByGroupId(string groupId, IEnumerable<string> userIds)
        {
            var ret = DbContextManager.Create<User>().SaveByGroupId(groupId, userIds);
            if (ret) CacheCleanUtility.ClearCache(userIds: userIds, groupIds: new List<string>() { groupId });
            return ret;
        }

        /// 根据用户名修改用户头像
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public static bool SaveUserIconByName(string userName, string iconName)
        {
            var ret = DbContextManager.Create<User>().SaveUserIconByName(userName, iconName);
            if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersDataKey}*");
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool SaveDisplayName(string userName, string displayName)
        {
            var ret = DbContextManager.Create<User>().SaveDisplayName(userName, displayName);
            if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersDataKey}*");
            return ret;
        }

        /// <summary>
        /// 根据用户名更改用户皮肤
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public static bool SaveUserCssByName(string userName, string cssName)
        {
            var ret = DbContextManager.Create<User>().SaveUserCssByName(userName, cssName);
            if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{UserHelper.RetrieveUsersDataKey}*");
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser RetrieveUserByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName), k => DbContextManager.Create<User>().RetrieveUserByUserName(userName), RetrieveUsersByNameDataKey);
    }
}
