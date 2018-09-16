using Longbow.Data;
using System;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DBAccessManager
    {
        private static readonly Lazy<IDBAccess> db = new Lazy<IDBAccess>(() => DBAccessFactory.CreateDB("ba"), true);

        public static IDBAccess SqlDBAccess
        {
            get { return db.Value; }
        }
    }
}
