using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

/// <summary>
/// 
/// </summary>
class NavigationService : BaseDatabase, INavigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public NavigationService(IDatabase db) => Database = db;

    /// <summary>
    /// 获得指定用户名可访问的所有菜单集合
    /// </summary>
    /// <param name="userName">当前用户名</param>
    /// <returns>未层次化的菜单集合</returns>
    public List<Navigation> GetAllMenus(string userName)
    {
        var order = Database.Provider.EscapeSqlIdentifier("Order");
        return Database.Fetch<Models.Navigation>($"select n.ID, n.ParentId, n.Name, n.{order}, n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = @Category and d.Define = 'System' left join Navigations ln on n.ParentId = ln.ID inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID ORDER BY n.Application, n.{order}", new { UserName = userName, Category = "菜单", RoleName = "Administrators" });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetMenusByRoleId(string? roleId) => Database.Fetch<string>("select NavigationID from NavigationRole where RoleID = @0", roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="menuIds"></param>
    /// <returns></returns>
    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from NavigationRole where RoleID = @0", roleId);
            Database.InsertBatch("NavigationRole", menuIds.Select(g => new { NavigationID = g, RoleID = roleId }));
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
