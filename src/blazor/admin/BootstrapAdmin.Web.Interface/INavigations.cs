using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.Interface
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
