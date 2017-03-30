using Longbow.Data;
using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DBAccessManager
    {
        private static readonly Lazy<DBAccess> db = new Lazy<DBAccess>(() => DBAccess.CreateDB("SQL"), true);

        public static DBAccess SqlDBAccess
        {
            get { return db.Value; }
        }
    }
}
