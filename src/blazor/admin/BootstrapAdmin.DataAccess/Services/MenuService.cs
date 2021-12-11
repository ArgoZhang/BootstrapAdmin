using BootstrapAdmin.DataAccess.Models;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuService : IMenu
    {
        private IDatabase _db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public MenuService(IDatabase db) => _db = db;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Menu> GetMenus()
        {
            throw new NotImplementedException();
        }
    }
}
