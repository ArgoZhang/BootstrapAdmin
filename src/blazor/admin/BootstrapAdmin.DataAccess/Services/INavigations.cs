using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface INavigations
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Navigations> RetrieveAllMenus(string userName);
    }
}
