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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Role> RetrieveRoles()
        {
            return MongoDbAccessManager.Roles.Find(FilterDefinition<DataAccess.Role>.Empty).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool SaveRole(DataAccess.Role p)
        {
            if (p.Id == "0")
            {
                p.Id = null;
                MongoDbAccessManager.Roles.InsertOne(p);
                return true;
            }
            else
            {
                MongoDbAccessManager.Roles.UpdateOne(md => md.Id == p.Id, Builders<DataAccess.Role>.Update.Set(md => md.RoleName, p.RoleName).Set(md => md.Description, p.Description));
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool DeleteRole(IEnumerable<string> value)
        {
            var list = new List<WriteModel<DataAccess.Role>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<DataAccess.Role>(Builders<DataAccess.Role>.Filter.Eq(g => g.Id, id)));
            }
            MongoDbAccessManager.Roles.BulkWrite(list);
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
            var user = UserHelper.RetrieveUsers().Cast<User>().FirstOrDefault(u => u.UserName == userName);
            var role = RoleHelper.RetrieveRoles();

            roles.AddRange(user.Roles.Select(r => role.FirstOrDefault(rl => rl.Id == r).RoleName));
            if (roles.Count == 0) roles.Add("Default");
            return roles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveRolesByUrl(string url)
        {
            // TODO: 需要菜单完成后处理此函数
            return new List<string>() { "Administrators" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Role> RetrieveRolesByUserId(string userId)
        {
            var roles = RoleHelper.RetrieveRoles();
            var user = UserHelper.RetrieveUsers().Cast<User>().FirstOrDefault(u => u.Id == userId);
            roles.ToList().ForEach(r => r.Checked = user.Roles.Any(id => id == r.Id) ? "checked" : "");
            return roles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveRolesByUserId(string userId, IEnumerable<string> roleIds)
        {
            MongoDbAccessManager.Users.FindOneAndUpdate(u => u.Id == userId, Builders<User>.Update.Set(u => u.Roles, roleIds));
            return true;
        }
    }
}
