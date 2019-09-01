using Longbow.Cache;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 角色数据相关操作帮助类
    /// </summary>
    internal class RoleHelper
    {
        /// <summary>
        /// 角色数据缓存键值
        /// </summary>
        public const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> Retrieves() => CacheManager.GetOrAdd(RetrieveRolesDataKey, key => DbManager.Roles.Find(FilterDefinition<Role>.Empty).ToList());

        /// <summary>
        /// 通过指定用户名获取角色集合方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUserName(string userName)
        {
            var roles = new List<string>();
            var user = UserHelper.Retrieves().FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                var role = Retrieves();
                roles.AddRange(role.Where(r => user.Roles.Any(rl => rl == r.Id)).Select(r => r.RoleName));
                if (roles.Count == 0) roles.Add("Default");
            }
            return roles;
        }
    }
}
