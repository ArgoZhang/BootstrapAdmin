using Bootstrap.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 菜单实体类
    /// </summary>
    public class Menu : DataAccess.Menu
    {
        /// <summary>
        /// 获取指定用户的所有菜单
        /// </summary>
        /// <param name="userName"></param>
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
                m.CategoryName = dicts.FirstOrDefault(d => d.Code == m.Category)?.Name ?? "";
                if (m.ParentId != "0") m.ParentName = menus.FirstOrDefault(p => p.Id == m.ParentId)?.Name ?? "";
            });

            return menus;
        }

        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(BootstrapMenu p)
        {
            if (string.IsNullOrEmpty(p.Id))
            {
                p.Id = null;
                DbManager.Menus.InsertOne(p);
                p.Id = DbManager.Menus.Find(m => m.Name == p.Name && m.Category == p.Category && m.ParentId == p.ParentId && m.Url == p.Url).FirstOrDefault().Id;
            }
            else
            {
                var update = Builders<BootstrapMenu>.Update.Set(md => md.ParentId, p.ParentId)
                    .Set(md => md.Name, p.Name)
                    .Set(md => md.Order, p.Order)
                    .Set(md => md.Icon, p.Icon)
                    .Set(md => md.Url, p.Url)
                    .Set(md => md.Category, p.Category)
                    .Set(md => md.Target, p.Target)
                    .Set(md => md.IsResource, p.IsResource)
                    .Set(md => md.Application, p.Application);
                DbManager.Menus.UpdateOne(md => md.Id == p.Id, update);
            }
            return true;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Delete(IEnumerable<string> value)
        {
            var list = new List<WriteModel<BootstrapMenu>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<BootstrapMenu>(Builders<BootstrapMenu>.Filter.Eq(g => g.Id, id)));
            }
            DbManager.Menus.BulkWrite(list);
            return true;
        }

        /// <summary>
        /// 获取指定角色相关菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveMenusByRoleId(string roleId) => DbManager.Roles.Find(md => md.Id == roleId).FirstOrDefault().Menus;

        /// <summary>
        /// 保存指定角色相关菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public override bool SaveMenusByRoleId(string roleId, IEnumerable<string> menuIds)
        {
            DbManager.Roles.FindOneAndUpdate(md => md.Id == roleId, Builders<Role>.Update.Set(md => md.Menus, menuIds));
            return true;
        }
    }
}
