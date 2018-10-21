using Longbow.Data;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbAccessManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static IDbAccess DbAccess { get { return DbAdapterManager.CreateDB(); } }
    }
}
