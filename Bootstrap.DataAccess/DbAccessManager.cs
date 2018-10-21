using Longbow.Data;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbAccessManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static IDbAccess DBAccess
        {
            get { return DbAdapterManager.CreateDB("ba"); }
        }
    }
}
