using Longbow.Cache;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    class RoleHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> Retrieves() => CacheManager.GetOrAdd(RetrieveRolesDataKey, key => DbManager.Roles.Find(FilterDefinition<Role>.Empty).ToList());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<string> RetrievesByUserName(string userName)
        {
            var roles = new List<string>();
            var user = UserHelper.Retrieves().FirstOrDefault(u => u.UserName.ToLowerInvariant() == userName.ToLowerInvariant());
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
