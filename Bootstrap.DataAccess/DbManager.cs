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
        private static readonly object _locker = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static IDatabase Create(string connectionName = null, bool keepAlive = false)
        {
            if (Mappers.GetMapper(typeof(Exceptions), null) == null)
            {
                lock (_locker)
                {
                    if (Mappers.GetMapper(typeof(Exceptions), null) == null) Mappers.Register(typeof(Exceptions).Assembly, new BootstrapDataAccessConventionMapper());
                }
            }
            var db = Longbow.Data.DbManager.Create(connectionName, keepAlive);
            db.ExceptionThrown += (sender, args) => args.Exception.Log(new NameValueCollection() { ["LastCmd"] = db.LastCommand });
            return db;
        }
    }
}
