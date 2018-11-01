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
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrieveGroups()
        {
            return MongoDbAccessManager.Groups.Find(FilterDefinition<Group>.Empty).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool SaveGroup(DataAccess.Group p)
        {
            if (p.Id == "0")
            {
                p.Id = null;
                MongoDbAccessManager.Groups.InsertOne(new Group() {
                    GroupName = p.GroupName,
                    Description = p.Description,
                    Roles = new List<string>()
                });
                return true;
            }
            else
            {
                MongoDbAccessManager.Groups.UpdateOne(md => md.Id == p.Id, Builders<Group>.Update.Set(md => md.GroupName, p.GroupName).Set(md => md.Description, p.Description));
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool DeleteGroup(IEnumerable<string> value)
        {
            var list = new List<WriteModel<Group>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<Group>(Builders<Group>.Filter.Eq(g => g.Id, id)));
            }
            MongoDbAccessManager.Groups.BulkWrite(list);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrieveGroupsByUserId(string userId)
        {
            var groups = GroupHelper.RetrieveGroups();
            var user = UserHelper.RetrieveUsers().Cast<User>().FirstOrDefault(u => u.Id == userId);
            groups.ToList().ForEach(g => g.Checked = user.Groups.Any(id => id == g.Id) ? "checked" : "");
            return groups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveGroupsByUserId(string userId, IEnumerable<string> groupIds)
        {
            MongoDbAccessManager.Users.FindOneAndUpdate(u => u.Id == userId, Builders<User>.Update.Set(u => u.Groups, groupIds));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrieveGroupsByRoleId(string roleId)
        {
            var groups = GroupHelper.RetrieveGroups().Cast<Group>().ToList();
            groups.ForEach(p => p.Checked = (p.Roles != null && p.Roles.Contains(roleId)) ? "checked" : "");
            return groups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveGroupsByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            var groups = MongoDbAccessManager.Groups.Find(md => md.Roles != null && md.Roles.Contains(roleId)).ToList();

            // Remove roles
            groups.ForEach(p =>
            {
                var roles = p.Roles == null ? new List<string>() : p.Roles.ToList();
                roles.Remove(roleId);
                MongoDbAccessManager.Groups.UpdateOne(md => md.Id == p.Id, Builders<Group>.Update.Set(md => md.Roles, roles));
            });

            groups = MongoDbAccessManager.Groups.Find(md => groupIds.Contains(md.Id)).ToList();
            // Add roles
            groups.ForEach(p =>
            {
                var roles = p.Roles == null ? new List<string>() : p.Roles.ToList();
                roles.Add(roleId);
                MongoDbAccessManager.Groups.UpdateOne(md => md.Id == p.Id, Builders<Group>.Update.Set(md => md.Roles, roles));
            });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveGroupsByUserName(string userName)
        {
            var groups = new List<string>();
            var user = UserHelper.RetrieveUsers().Cast<User>().FirstOrDefault(u => u.UserName == userName);
            var group = GroupHelper.RetrieveGroups();

            groups.AddRange(user.Groups.Select(r => group.FirstOrDefault(rl => rl.Id == r).GroupName));
            if (groups.Count == 0) groups.Add("Default");
            return groups;
        }
    }
}
