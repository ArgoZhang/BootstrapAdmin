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
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrieveGroups()
        {
            return MongoDbAccessManager.Groups.Find(FilterDefinition<DataAccess.Group>.Empty).ToList();
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
                MongoDbAccessManager.Groups.InsertOne(p);
                return true;
            }
            else
            {
                MongoDbAccessManager.Groups.UpdateOne(md => md.Id == p.Id, Builders<DataAccess.Group>.Update.Set(md => md.GroupName, p.GroupName).Set(md => md.Description, p.Description));
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
            var list = new List<WriteModel<DataAccess.Group>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<DataAccess.Group>(Builders<DataAccess.Group>.Filter.Eq(g => g.Id, id)));
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
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveGroupsByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            return base.SaveGroupsByRoleId(roleId, groupIds);
        }
    }
}
