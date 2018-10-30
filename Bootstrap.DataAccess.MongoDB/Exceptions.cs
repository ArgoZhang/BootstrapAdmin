using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Exceptions : DataAccess.Exceptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Exceptions> RetrieveExceptions()
        {
            var msg = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.Exceptions>("Exceptions");
            return msg.Find(ex => ex.LogTime >= DateTime.Now.AddDays(-7)).ToList();
        }
    }
}
