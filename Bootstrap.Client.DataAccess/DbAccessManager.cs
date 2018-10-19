using Longbow.Data;
using System;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbAccessManager
    {
        private static readonly Lazy<IDbAccess> _db = new Lazy<IDbAccess>(() => DbAccessFactory.CreateDB("sql"), true);
        /// <summary>
        /// 
        /// </summary>
        public static IDbAccess DbAccess { get { return _db.Value; } }
    }
}
