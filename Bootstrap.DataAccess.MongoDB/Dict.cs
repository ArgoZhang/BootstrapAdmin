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
        public override IEnumerable<BootstrapDict> RetrieveDicts() => MongoDbAccessManager.Dicts.Find(FilterDefinition<BootstrapDict>.Empty).ToList();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool DeleteDict(IEnumerable<string> value)
        {
            var list = new List<WriteModel<BootstrapDict>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<BootstrapDict>(Builders<BootstrapDict>.Filter.Eq(md => md.Id, id)));
            }
            MongoDbAccessManager.Dicts.BulkWrite(list);
            return true;
        }
    }
}
