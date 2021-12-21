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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<string> GetMenusByRoleId(string? roleId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool SaveMenusByRoleId(string? roleId, List<string> menuIds);
    }
}
