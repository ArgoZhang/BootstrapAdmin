using Bootstrap.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 菜单实体类
    /// </summary>
    internal class Menu : DataAccess.Menu
    {
        /// <summary>
        /// 通过当前用户名获得所有菜单
        /// </summary>
        /// <param name="userName">当前登录的用户名</param>
        /// <returns></returns>
        public override IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName)
        {
            var user = UserHelper.RetrieveUserByUserName(userName) as User;
            if (user == null) return Enumerable.Empty<BootstrapMenu>();

            var roles = RoleHelper.Retrieves();
            var groups = DbManager.Groups.Find(FilterDefinition<Group>.Empty).ToList();

            // 通过用户获取 组列表相关联的角色列表
            var userRoles = user.Groups.Aggregate(user.Roles.ToList(), (r, g) =>
            {
                var groupRoles = groups.FirstOrDefault(group => group.Id == g)?.Roles;
                if (groupRoles != null) r.AddRange(groupRoles);
                return r;
            }).Distinct().ToList();

            var allRoles = roles.Where(r => userRoles.Any(rl => rl == r.Id));
            var menus = DbManager.Menus.Find(FilterDefinition<BootstrapMenu>.Empty).ToList();

            // check administrators
            if (!allRoles.Any(r => r.RoleName.Equals("Administrators", StringComparison.OrdinalIgnoreCase)))
            {
                menus = menus.Where(m => allRoles.Any(r => r.Menus.Any(rm => rm == m.Id))).ToList();
            }

            var dicts = DictHelper.RetrieveDicts().Where(m => m.Category == "菜单");
            menus.ForEach(m =>
            {
                m.CategoryName = dicts.FirstOrDefault(d => d.Code == m.Category)?.Name ?? "";
                if (m.ParentId != "0") m.ParentName = menus.FirstOrDefault(p => p.Id == m.ParentId)?.Name ?? "";
            });

            return menus;
        }
    }
}
