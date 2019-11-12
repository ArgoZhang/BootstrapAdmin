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
        public IEnumerable<string> Menus { get; set; } = new string[0];

        /// <summary>
        /// 此角色关联的所有应用程序
        /// </summary>
        public IEnumerable<string> Apps { get; set; } = new string[0];

        /// <summary>
        /// 获得/设置 角色主键ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 角色名称
        /// </summary>
        public string RoleName { get; set; } = "";

        /// <summary>
        /// 通过指定登录名获取授权角色名称数据集合
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUserName(string userName)
        {
            var user = UserHelper.RetrieveUserByUserName(userName) as User;
            return user == null ? new string[0] : RoleHelper.Retrieves().Where(r => user.Roles.Any(ur => ur == r.Id)).Select(r => r.RoleName);
        }

        /// <summary>
        /// 根据菜单url查询某个所拥有的角色
        /// 从NavigatorRole表查
        /// 从Navigators -> GroupNavigatorRole -> Role查查询某个用户所拥有的角色
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUrl(string url, string appId)
        {
            var menu = DbManager.Menus.Find(md => md.Url.StartsWith(url)).FirstOrDefault();
            var ret = RoleHelper.Retrieves().Where(md => md.Menus.Any(m => m == menu.Id) && md.Apps.Any(m => m.Equals(appId, StringComparison.OrdinalIgnoreCase))).Select(m => m.RoleName).ToList();
            if (!ret.Any(r => r.Equals("Administrators", StringComparison.OrdinalIgnoreCase))) ret.Add("Administrators");
            return ret;
        }
    }
}
