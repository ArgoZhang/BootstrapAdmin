using Longbow.Data;
using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DBAccessManager
    {
        private static readonly Lazy<IDbAccess> db = new Lazy<IDbAccess>(() => DbAccessFactory.CreateDB("ba"), true);
        /// <summary>
        /// 
        /// </summary>
        public static IDbAccess DBAccess
        {
            get { return db.Value; }
        }
    }
}
