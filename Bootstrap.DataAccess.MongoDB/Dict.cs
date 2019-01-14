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
        public override bool Delete(IEnumerable<string> value)
        {
            var list = new List<WriteModel<BootstrapDict>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<BootstrapDict>(Builders<BootstrapDict>.Filter.Eq(md => md.Id, id)));
            }
            MongoDbAccessManager.Dicts.BulkWrite(list);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(BootstrapDict p)
        {
            if (p.Id == "0")
            {
                p.Id = null;
                MongoDbAccessManager.Dicts.InsertOne(p);
                return true;
            }
            else
            {
                MongoDbAccessManager.Dicts.UpdateOne(md => md.Id == p.Id, Builders<BootstrapDict>.Update.Set(md => md.Category, p.Category)
                    .Set(md => md.Define, p.Define)
                    .Set(md => md.Name, p.Name)
                    .Set(md => md.Code, p.Code));
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public override bool SaveSettings(BootstrapDict dict)
        {
            MongoDbAccessManager.Dicts.FindOneAndUpdate(md => md.Category == dict.Category && md.Name == dict.Name, Builders<BootstrapDict>.Update.Set(md => md.Code, dict.Code));
            return true;
        }
    }
}
