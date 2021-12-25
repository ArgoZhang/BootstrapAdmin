namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IApp
{
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
