using Longbow.Cache;
using MongoDB.Driver;
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
    }
}
