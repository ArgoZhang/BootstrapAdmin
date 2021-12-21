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
    /// <param name="userId"></param>
    /// <returns></returns>
    List<string> GetRolesByGroupId(string? userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    List<string> GetRolesByUserId(string? groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByUserId(string? groupId, IEnumerable<string> roleIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    List<string> GetRolesByMenuId(string? menuId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds);
}
