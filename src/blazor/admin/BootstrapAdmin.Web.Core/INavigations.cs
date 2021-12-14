using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core
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
