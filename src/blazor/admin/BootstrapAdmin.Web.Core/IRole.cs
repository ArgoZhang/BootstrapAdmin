using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IRole
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Role> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    List<string> GetUsersByRoleId(string? id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    bool SaveUsersByRoleId(string? id, IEnumerable<string> values);
}
