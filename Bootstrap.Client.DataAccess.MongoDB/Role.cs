using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 角色实体类
    /// </summary>
    internal class Role : DataAccess.Role
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
        /// 通过指定登录名获取授权角色名称数据集合
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUserName(string userName)
        {
            var roles = new List<string>();
            var user = UserHelper.Retrieves().FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
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
            var ret = RoleHelper.Retrieves().Where(md => md.Menus != null && md.Menus.Any(m => m.Equals(menu.Id, StringComparison.OrdinalIgnoreCase))).Select(m => m.RoleName).ToList();
            if (!ret.Any(r => r.Equals("Administrators", StringComparison.OrdinalIgnoreCase))) ret.Add("Administrators");
            return ret;
        }
    }
}
