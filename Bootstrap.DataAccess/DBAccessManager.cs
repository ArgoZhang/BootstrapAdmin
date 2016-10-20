using Longbow.Data;
using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    static class DBAccessManager
    {
        private static Lazy<DBAccess> db = new Lazy<DBAccess>(() => DBAccess.CreateDB("SQL"), true);

        public static DBAccess SqlDBAccess
        {
            get { return db.Value; }
        }
    }
}
