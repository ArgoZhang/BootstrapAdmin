using MongoDB.Driver;
using System.Collections.Generic;

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
            var groups = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.Group>("Groups");
            return groups.Find(FilterDefinition<DataAccess.Group>.Empty).ToList();
        }
    }
}
