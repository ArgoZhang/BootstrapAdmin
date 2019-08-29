using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Role : DataAccess.Role
    {
        /// <summary>
        /// 此角色关联的所有菜单
        /// </summary>
        public IEnumerable<string> Menus { get; set; }

        /// <summary>
        /// 此角色关联的所有应用程序
        /// </summary>
        public IEnumerable<string> Apps { get; set; }

        /// <summary>
        /// 获得/设置 角色主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获得/设置 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 获得/设置 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUserName(string userName)
        {
            var roles = new List<string>();
            var user = UserHelper.Retrieves().Cast<User>().FirstOrDefault(u => u.UserName.ToLowerInvariant() == userName.ToLowerInvariant());
            if (user != null)
            {
                var role = RoleHelper.Retrieves();
                roles.AddRange(role.Where(r => user.Roles.Any(rl => rl == r.Id)).Select(r => r.RoleName));
                if (roles.Count == 0) roles.Add("Default");
            }
            return roles;
        }

        /// <summary>
        /// 根据菜单url查询某个所拥有的角色
        /// 从NavigatorRole表查
        /// 从Navigators -> GroupNavigatorRole -> Role查查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUrl(string url)
        {
            var menu = DbManager.Menus.Find(md => md.Url.StartsWith(url)).FirstOrDefault();
            var ret = RoleHelper.Retrieves().Cast<Role>().Where(md => md.Menus != null && md.Menus.Contains(menu.Id)).Select(m => m.RoleName).ToList();
            if (!ret.Contains("Administrators")) ret.Add("Administrators");
            return ret;
        }
    }
}
