using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu : DataAccess.Menu
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName)
        {
            var menus = MongoDbAccessManager.DBAccess.GetCollection<BootstrapMenu>("Navigations");
            return menus.Find(FilterDefinition<BootstrapMenu>.Empty).ToList();
        }
    }
}
