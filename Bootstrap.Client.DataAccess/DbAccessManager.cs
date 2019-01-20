using Longbow.Data;
using PetaPoco;

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
        public static IDatabase DbAccess { get { return DbManager.Create(); } }
    }
}
