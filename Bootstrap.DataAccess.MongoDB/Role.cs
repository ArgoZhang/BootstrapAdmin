using MongoDB.Driver;
using System.Collections.Generic;

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
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveRolesByUserName(string userName)
        {
            return new List<string>() { "Administrators" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveRolesByUrl(string url) => new List<string>() { "Administrators" };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Role> RetrieveRoles()
        {
            var roles = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.Role>("Roles");
            return roles.Find(FilterDefinition<DataAccess.Role>.Empty).ToList();
        }
    }
}
