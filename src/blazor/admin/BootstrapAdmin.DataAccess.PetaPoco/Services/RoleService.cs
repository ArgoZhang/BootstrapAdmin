using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class RoleService : BaseDatabase, IRole
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public RoleService(IDatabase db) => Database = db;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Role> GetAll() => Database.Fetch<Role>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public List<string> GetRolesByGroupId(string? groupId) => Database.Fetch<string>("select RoleID from RoleGroup where GroupID = @0", groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from RoleGroup where GroupID = @0", groupId);
            Database.InsertBatch("RoleGroup", roleIds.Select(g => new { RoleID = g, GroupID = groupId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> GetRolesByUserId(string? userId) => Database.Fetch<string>("select RoleID from UserRole where UserID = @0", userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from UserRole where UserID = @0", userId);
            Database.InsertBatch("UserRole", roleIds.Select(g => new { RoleID = g, UserID = userId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }
        return ret;
    }

    public List<string> GetRolesByMenuId(string? menuId) => Database.Fetch<string>("select RoleID from NavigationRole where NavigationID = @0", menuId);

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from NavigationRole where NavigationID = @0", menuId);
            Database.InsertBatch("NavigationRole", roleIds.Select(g => new { RoleID = g, NavigationID = menuId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }
        return ret;
    }
}
