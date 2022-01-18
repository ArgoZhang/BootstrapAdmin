using BootstrapClient.DataAccess.Models;

namespace BootstrapClient.Web.Core;

/// <summary>
/// 
/// </summary>
public interface INavigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Navigation> GetMenus(string userName);
}
