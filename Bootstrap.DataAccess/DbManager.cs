using PetaPoco;

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
            return db.AddMaps();
        }

        private static IDatabase AddMaps(this IDatabase database)
        {
            database.AddMap<Dict>("Dicts");
            database.AddMap<User>("Users", new string[] { "Checked", "Period", "NewPassword", "UserStatus" });
            database.AddMap<Exceptions>("Exceptions", new string[] { "Period" });
            database.AddMap<Group>("Groups", new string[] { "Checked" });
            database.AddMap<Log>("Logs");
            database.AddMap<Menu>("Navigations", new string[] { "ParentName", "CategoryName", "Active", "Menus" });
            database.AddMap<Role>("Roles", new string[] { "Checked" });
            database.AddMap<Task>("Tasks");
            database.AddMap<Trace>("Traces");
            return database;
        }
    }
}
