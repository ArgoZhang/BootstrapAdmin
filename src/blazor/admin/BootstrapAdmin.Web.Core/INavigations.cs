using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.Core
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
        List<Navigation> RetrieveAllMenus(string userName);
    }
}
