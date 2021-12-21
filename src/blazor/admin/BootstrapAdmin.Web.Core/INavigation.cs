using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Navigation> GetAllMenus(string userName);
    }
}
