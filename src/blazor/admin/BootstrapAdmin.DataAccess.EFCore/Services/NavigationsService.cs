using BootstrapAdmin.Web.Core;
using BootstrapAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

/// <summary>
/// 
/// </summary>
class NavigationsService : INavigation
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factory"></param>
    public NavigationsService(IDbContextFactory<BootstrapAdminContext> factory) => DbFactory = factory;

    /// <summary>
    /// 获得指定用户名可访问的所有菜单集合
    /// </summary>
    /// <param name="userName">当前用户名</param>
    /// <returns>未层次化的菜单集合</returns>
    public List<Navigation> GetAllMenus(string userName)
    {
        using var context = DbFactory.CreateDbContext();
        //return context..Fetch<Models.Navigations>($"select n.ID, n.ParentId, n.Name, n.{order}, n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application, d.Name as CategoryName, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = @Category and d.Define = 0 left join Navigations ln on n.ParentId = ln.ID inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID", new { UserName = userName, Category = "菜单", RoleName = "Administrators" });
        return new List<Navigation>();
    }

    public List<string> GetMenusByRoleId(string? roleId)
    {
        throw new NotImplementedException();
    }

    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        throw new NotImplementedException();
    }
}
