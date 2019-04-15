using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        private static bool UserChecker(User user)
        {
            if (user.Description?.Length > 500) user.Description = user.Description.Substring(0, 500);
            if (user.UserName?.Length > 16) user.UserName = user.UserName.Substring(0, 16);
            if (user.Password?.Length > 16) user.Password = user.Password.Substring(0, 16);
            if (user.DisplayName?.Length > 20) user.DisplayName = user.DisplayName.Substring(0, 20);
            var pattern = @"^[a-zA-Z0-9_@.]*$";
            return user.UserName.IsNullOrEmpty() || Regex.IsMatch(user.UserName, pattern);
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> Retrieves() => CacheManager.GetOrAdd(RetrieveUsersDataKey, key => DbContextManager.Create<User>().Retrieves());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool Authenticate(string userName, string password, Action<LoginUser> config)
        {
            if (!UserChecker(new User { UserName = userName, Password = password })) return false;
            var loginUser = new LoginUser
            {
                UserName = userName,
                LoginTime = DateTime.Now,
                Result = "登录失败"
            };
            config(loginUser);
            var ret = DbContextManager.Create<User>().Authenticate(userName, password);
            if (ret) loginUser.Result = "登录成功";
            LoginHelper.Log(loginUser);
            return ret;
        }

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
            var admins = Retrieves().Where(u => u.UserName.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            value = value.Where(v => !admins.Any(u => u.Id == v));
            if (!value.Any()) return true;
            var ret = DbContextManager.Create<User>().Delete(value);
            if (ret) CacheCleanUtility.ClearCache(userIds: value);
            return ret;
        }

        /// <summary>
        /// 保存用户默认App
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static bool SaveApp(string userName, string app)
        {
            var ret = DbContextManager.Create<User>().SaveApp(userName, app);
            if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersDataKey}*");
            return ret;
        }

        /// <summary>
        /// 保存新建
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Save(User user)
        {
            if (!UserChecker(user)) return false;

            if (DictHelper.RetrieveSystemModel() && !user.Id.IsNullOrEmpty())
            {
                var admins = Retrieves().Where(u => u.UserName.Equals("Admin", StringComparison.OrdinalIgnoreCase));
                if (admins.Any(v => v.Id == user.Id)) return true;
            }
            var ret = DbContextManager.Create<User>().Save(user);
            if (ret) CacheCleanUtility.ClearCache(userIds: string.IsNullOrEmpty(user.Id) ? new List<string>() : new List<string>() { user.Id });
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
            if (!UserChecker(new User { Password = password, DisplayName = displayName })) return false;
            if (DictHelper.RetrieveSystemModel())
            {
                var admins = Retrieves().Where(u => u.UserName.Equals("Admin", StringComparison.OrdinalIgnoreCase));
                if (admins.Any(v => v.Id == id)) return true;
            }
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
        public static bool ChangePassword(string userName, string password, string newPass)
        {
            if (!UserChecker(new User { UserName = userName, Password = password })) return false;
            if (DictHelper.RetrieveSystemModel() && userName.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return true;
            return DbContextManager.Create<User>().ChangePassword(userName, password, newPass);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ResetPassword(string userName, string password)
        {
            if (!UserChecker(new User { UserName = userName, Password = password })) return false;
            if (DictHelper.RetrieveSystemModel() && userName.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return true;
            return DbContextManager.Create<User>().ResetPassword(userName, password);
        }

        /// <summary>
        /// 忘记密码调用
        /// </summary>
        /// <param name="user"></param>
        public static bool ForgotPassword(ResetUser user) => DbContextManager.Create<User>().ForgotPassword(user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rejectBy"></param>
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

        /// <summary>
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
            if (!UserChecker(new User { UserName = userName, DisplayName = displayName })) return false;
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
            if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersByNameDataKey}*");
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser RetrieveUserByUserName(string userName) => CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName), k => DbContextManager.Create<User>().RetrieveUserByUserName(userName), RetrieveUsersByNameDataKey);

        /// <summary>
        /// 通过登录账号获得用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static ResetUser RetrieveResetUserByUserName(string userName) => DbContextManager.Create<ResetUser>().RetrieveUserByUserName(userName);

        /// <summary>
        /// 通过登录账户获得重置密码原因
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<DateTime, string>> RetrieveResetReasonsByUserName(string userName) => DbContextManager.Create<ResetUser>().RetrieveResetReasonsByUserName(userName);
    }
}
