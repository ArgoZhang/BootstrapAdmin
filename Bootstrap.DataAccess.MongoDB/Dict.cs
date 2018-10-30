using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Dict : DataAccess.Dict
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<BootstrapDict> RetrieveDicts()
        {
            var dicts = MongoDbAccessManager.DBAccess.GetCollection<Dict>("Dicts");
            return dicts.Find(FilterDefinition<Dict>.Empty).ToList();
        }
    }
}
