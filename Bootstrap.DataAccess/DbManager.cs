using PetaPoco;
using System;
using System.Collections.Specialized;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static IDatabase Create(string connectionName = null, bool keepAlive = false)
        {
            var db = Longbow.Data.DbManager.Create(connectionName, keepAlive);
            db.ExceptionThrown += (sender, args) => args.Exception.Log(new NameValueCollection() { ["LastCmd"] = db.LastCommand });
            return db;
        }
    }
}
