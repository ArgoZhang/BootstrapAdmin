using Longbow.Cache;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    class GroupHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveGroupsDataKey = "GroupHelper-RetrieveGroups";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Group> Retrieves()
        {
            return CacheManager.GetOrAdd(RetrieveGroupsDataKey, key => DbManager.Groups.Find(FilterDefinition<Group>.Empty).ToList());
        }
    }
}
