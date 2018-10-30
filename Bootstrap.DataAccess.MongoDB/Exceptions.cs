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
        private static void ClearExceptions()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                MongoDbAccessManager.Exceptions.DeleteMany(ex => ex.LogTime < DateTime.Now.AddDays(-7));
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Exceptions> RetrieveExceptions()
        {
            return MongoDbAccessManager.Exceptions.Find(ex => ex.LogTime >= DateTime.Now.AddDays(-7)).ToList();
        }
    }
}
