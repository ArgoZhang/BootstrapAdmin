using MongoDB.Driver;
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
        public IEnumerable<string> Menus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
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
            if (p.Id == "0")
            {
                p.Id = null;
                DbManager.Roles.InsertOne(new Role()
                {
                    RoleName = p.RoleName,
                    Description = p.Description,
                    Menus = new List<string>()
                });
                return true;
            }
            else
            {
                DbManager.Roles.UpdateOne(md => md.Id == p.Id, Builders<Role>.Update.Set(md => md.RoleName, p.RoleName).Set(md => md.Description, p.Description));
                return true;
            }
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
        public override IEnumerable<string> RetrieveRolesByUserName(string userName)
        {
            var roles = new List<string>();
            var user = UserHelper.Retrieves().Cast<User>().FirstOrDefault(u => u.UserName == userName);
            var role = RoleHelper.Retrieves();

            roles.AddRange(user.Roles.Select(r => role.FirstOrDefault(rl => rl.Id == r).RoleName));
            if (roles.Count == 0) roles.Add("Default");
            return roles;
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
            roles.ForEach(r => r.Menus = null);
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
            var roles = DbManager.Roles.Find(md => md.Menus != null && md.Menus.Contains(menuId)).ToList();

            // Remove roles
            roles.ForEach(p =>
            {
                var menus = p.Menus == null ? new List<string>() : p.Menus.ToList();
                menus.Remove(menuId);
                DbManager.Roles.UpdateOne(md => md.Id == p.Id, Builders<Role>.Update.Set(md => md.Menus, menus));
            });

            roles = DbManager.Roles.Find(md => roleIds.Contains(md.Id)).ToList();
            roles.ForEach(role =>
            {
                var menus = role.Menus == null ? new List<string>() : role.Menus.ToList();
                if (!menus.Contains(menuId))
                {
                    menus.Add(menuId);
                    DbManager.Roles.UpdateOne(md => md.Id == role.Id, Builders<Role>.Update.Set(md => md.Menus, menus));
                }
            });
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
        /// <returns></returns>
        public override IEnumerable<string> RetrieveRolesByUrl(string url)
        {
            var menu = DbManager.Menus.Find(md => md.Url.StartsWith(url)).FirstOrDefault();
            var ret = RoleHelper.Retrieves().Cast<Role>().Where(md => md.Menus != null && md.Menus.Contains(menu.Id)).Select(m => m.RoleName).ToList();
            if (!ret.Contains("Administrators")) ret.Add("Administrators");
            return ret;
        }
    }
}
