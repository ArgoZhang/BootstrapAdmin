using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Menu> GetMenus();
    }
}
