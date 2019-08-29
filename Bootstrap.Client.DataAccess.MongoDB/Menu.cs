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
    public class Menu : DataAccess.Menu
    {
        /// <summary>
        /// 通过当前用户名获得所有菜单
        /// </summary>
        /// <param name="userName">当前登录的用户名</param>
        /// <returns></returns>
        public override IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName)
        {
            var user = UserHelper.Retrieves().Cast<User>().FirstOrDefault(u => u.UserName.ToLowerInvariant() == userName.ToLowerInvariant());
            if (user == null) return Enumerable.Empty<BootstrapMenu>();

            var roles = RoleHelper.Retrieves().Cast<Role>();
            var groups = GroupHelper.Retrieves().Cast<Group>();

            // 通过用户获取 组列表相关联的角色列表
            var userRoles = user.Groups.Aggregate(user.Roles.ToList(), (r, g) =>
            {
                var groupRoles = groups.Where(group => group.Id == g).FirstOrDefault()?.Roles;
                if (groupRoles != null) r.AddRange(groupRoles);
                return r;
            }).Distinct().ToList();

            var allRoles = roles.Where(r => userRoles.Any(rl => rl == r.Id)).ToList();
            var menus = DbManager.Menus.Find(FilterDefinition<BootstrapMenu>.Empty).ToList()
                .Where(m => allRoles.Any(r => r.RoleName.Equals("Administrators", StringComparison.OrdinalIgnoreCase) || r.Menus.Any(rm => rm.Equals(m.Id, StringComparison.OrdinalIgnoreCase))))
                .ToList();

            var dicts = DictHelper.RetrieveDicts().Where(m => m.Category == "菜单");
            menus.ForEach(m =>
            {
                m.CategoryName = dicts.FirstOrDefault(d => d.Code == m.Category)?.Name;
                if (m.ParentId != "0") m.ParentName = menus.FirstOrDefault(p => p.Id == m.ParentId)?.Name;
            });

            return menus;
        }
    }
}
