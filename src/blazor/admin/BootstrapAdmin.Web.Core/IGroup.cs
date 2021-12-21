using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IGroup
{

    /// <summary>
    /// 获得所有用户
    /// </summary>
    /// <returns></returns>
    List<Group> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    List<string> GetGroupsByUserId(string? userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds);
}
