using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Group : DataAccess.Group
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> Retrieves()
        {
            return DbManager.Groups.Find(FilterDefinition<Group>.Empty).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.Group p)
        {
            if (string.IsNullOrEmpty(p.Id))
            {
                p.Id = null;
                DbManager.Groups.InsertOne(new Group()
                {
                    GroupCode = p.GroupCode,
                    GroupName = p.GroupName,
                    Description = p.Description,
                    Roles = new List<string>()
                });
                p.Id = DbManager.Groups.Find(g => g.GroupName == p.GroupName && g.Description == p.Description).FirstOrDefault().Id;
            }
            else
            {
                DbManager.Groups.UpdateOne(md => md.Id == p.Id, Builders<Group>.Update.Set(md => md.GroupName, p.GroupName).Set(md => md.Description, p.Description));
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
            var list = new List<WriteModel<Group>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<Group>(Builders<Group>.Filter.Eq(g => g.Id, id)));
            }
            DbManager.Groups.BulkWrite(list);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrievesByUserId(string userId)
        {
            var groups = GroupHelper.Retrieves();
            var user = UserHelper.Retrieves().Cast<User>().FirstOrDefault(u => u.Id == userId);
            if (user != null)
                groups.ToList().ForEach(g => g.Checked = user.Groups.Any(id => id == g.Id) ? "checked" : "");
            return groups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveByUserId(string userId, IEnumerable<string> groupIds)
        {
            DbManager.Users.FindOneAndUpdate(u => u.Id == userId, Builders<User>.Update.Set(u => u.Groups, groupIds));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrievesByRoleId(string roleId)
        {
            var groups = GroupHelper.Retrieves().Cast<Group>().ToList();
            groups.ForEach(p => p.Checked = (p.Roles != null && p.Roles.Contains(roleId)) ? "checked" : "");
            return groups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            var groups = DbManager.Groups.Find(md => md.Roles != null && md.Roles.Contains(roleId)).ToList();

            // Remove roles
            groups.ForEach(p =>
            {
                var roles = p.Roles == null ? new List<string>() : p.Roles.ToList();
                roles.Remove(roleId);
                DbManager.Groups.UpdateOne(md => md.Id == p.Id, Builders<Group>.Update.Set(md => md.Roles, roles));
            });

            groups = DbManager.Groups.Find(md => groupIds.Contains(md.Id)).ToList();
            // Add roles
            groups.ForEach(p =>
            {
                var roles = p.Roles == null ? new List<string>() : p.Roles.ToList();
                roles.Add(roleId);
                DbManager.Groups.UpdateOne(md => md.Id == p.Id, Builders<Group>.Update.Set(md => md.Roles, roles));
            });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<BootstrapGroup> RetrievesByUserName(string userName)
        {
            var groups = new List<BootstrapGroup>();
            var group = GroupHelper.Retrieves();
            var user = UserHelper.Retrieves().Cast<User>().FirstOrDefault(u => u.UserName == userName);
            if (user != null)
                groups.AddRange(group.Where(g => user.Groups.Any(ug => ug == g.Id)));
            return groups;
        }
    }
}
