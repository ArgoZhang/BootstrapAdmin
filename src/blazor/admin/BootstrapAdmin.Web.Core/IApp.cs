using BootstrapBlazor.Components;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IApp
{
    /// <summary>
    /// 获得所有用户
    /// </summary>
    /// <returns></returns>
    List<SelectedItem> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<string> GetAppsByRoleId(string? roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="appIds"></param>
    /// <returns></returns>
    bool SaveAppsByRoleId(string? roleId, IEnumerable<string> appIds);
}
