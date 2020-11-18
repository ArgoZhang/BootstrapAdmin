using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Role : DataAccess.Role
    {
        /// <summary>
        /// 此角色关联的所有菜单
        /// </summary>
        public IEnumerable<string> Menus { get; set; } = new List<string>();

        /// <summary>
        /// 此角色关联的所有应用程序
        /// </summary>
        public IEnumerable<string> Apps { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Role> Retrieves()
        {
            return DbManager.Roles.Find(FilterDefinition<Role>.Empty).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.Role p)
        {
            if (string.IsNullOrEmpty(p.Id))
            {
                p.Id = null;
                DbManager.Roles.InsertOne(new Role()
                {
                    RoleName = p.RoleName,
                    Description = p.Description
                });
                p.Id = DbManager.Roles.Find(r => r.RoleName == p.RoleName && r.Description == p.Description).FirstOrDefault().Id;
            }
            else
            {
                DbManager.Roles.UpdateOne(md => md.Id == p.Id, Builders<Role>.Update.Set(md => md.RoleName, p.RoleName).Set(md => md.Description, p.Description));
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Delete(IEnumerable<string> value)
        {
            var list = new List<WriteModel<Role>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<Role>(Builders<Role>.Filter.Eq(g => g.Id, id)));
            }
            DbManager.Roles.BulkWrite(list);
            return true;
        }

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
                // 用户所属角色
                var role = RoleHelper.Retrieves();
                roles.AddRange(role.Where(r => user.Roles.Any(rl => rl == r.Id)).Select(r => r.RoleName));

                // 用户所属部门 部门所属角色
                GroupHelper.Retrieves().Cast<Group>().Where(group => user.Groups.Any(g => g == group.Id)).ToList().ForEach(g =>
                {
                    roles.AddRange(role.Where(r => g.Roles.Any(rl => rl == r.Id)).Select(r => r.RoleName));
                });

                if (roles.Count == 0) roles.Add("Default");
            }
            return roles.Distinct();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Role> RetrievesByUserId(string userId)
        {
            var roles = RoleHelper.Retrieves();
            var user = UserHelper.Retrieves().Cast<User>().FirstOrDefault(u => u.Id == userId);
            if (user != null)
                roles.ToList().ForEach(r => r.Checked = user.Roles.Any(id => id == r.Id) ? "checked" : "");
            return roles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveByUserId(string userId, IEnumerable<string> roleIds)
        {
            DbManager.Users.FindOneAndUpdate(u => u.Id == userId, Builders<User>.Update.Set(u => u.Roles, roleIds));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Role> RetrievesByMenuId(string menuId)
        {
            var roles = RoleHelper.Retrieves().Cast<Role>().ToList();
            roles.ForEach(r => r.Checked = (r.Menus != null && r.Menus.Contains(menuId)) ? "checked" : "");
            roles.ForEach(r => r.Menus = new List<string>());
            return roles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SavaByMenuId(string menuId, IEnumerable<string> roleIds)
        {
            // 参数 id 可能是子菜单
            // https://gitee.com/LongbowEnterprise/dashboard/issues?id=IQW93
            if (!string.IsNullOrEmpty(menuId))
            {
                // 找到所有包含此菜单的角色集合
                var roles = DbManager.Roles.Find(md => md.Menus != null && md.Menus.Contains(menuId)).ToList();

                // 所有角色集合移除此菜单
                roles.ForEach(p =>
                {
                    var menus = p.Menus.ToList();
                    menus.Remove(menuId);
                    DbManager.Roles.UpdateOne(md => md.Id == p.Id, Builders<Role>.Update.Set(md => md.Menus, menus));
                });

                // 授权角色集合
                string? parentId = menuId;
                roles = DbManager.Roles.Find(md => roleIds.Contains(md.Id)).ToList();
                do
                {
                    roles.ForEach(role =>
                    {
                        var menus = role.Menus.ToList();
                        if (!menus.Contains(parentId))
                        {
                            menus.Add(parentId);
                            DbManager.Roles.UpdateOne(md => md.Id == role.Id, Builders<Role>.Update.Set(md => md.Menus, menus));
                        }
                    });

                    // 查找父级菜单
                    parentId = DbManager.Menus.Find(md => md.Id == parentId).FirstOrDefault()?.ParentId;
                }
                while (!string.IsNullOrEmpty(parentId) && parentId != "0");
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Role> RetrievesByGroupId(string groupId)
        {
            var roles = RoleHelper.Retrieves();
            var group = GroupHelper.Retrieves().Cast<Group>().FirstOrDefault(u => u.Id == groupId);
            if (group != null)
                roles.ToList().ForEach(r => r.Checked = group.Roles.Any(id => id == r.Id) ? "checked" : "");
            return roles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveByGroupId(string groupId, IEnumerable<string> roleIds)
        {
            DbManager.Groups.FindOneAndUpdate(u => u.Id == groupId, Builders<Group>.Update.Set(u => u.Roles, roleIds));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUrl(string url, string appId)
        {
            var menu = DbManager.Menus.Find(md => md.Url.StartsWith(url)).FirstOrDefault();
            var ret = RoleHelper.Retrieves().Cast<Role>().Where(md => md.Menus != null && md.Menus.Any(m => m.Equals(menu.Id, StringComparison.OrdinalIgnoreCase)) && md.Apps.Contains(appId)).Select(m => m.RoleName).ToList();
            if (!ret.Contains("Administrators")) ret.Add("Administrators");
            return ret;
        }
    }
}
