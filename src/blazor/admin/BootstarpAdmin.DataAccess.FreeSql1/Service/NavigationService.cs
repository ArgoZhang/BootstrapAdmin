// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.FreeSql.Service;

class NavigationService : INavigation
{
    private IFreeSql FreeSql { get; }

    public NavigationService(IFreeSql freeSql) => FreeSql = freeSql;

    public List<Navigation> GetAllMenus(string userName)
    {
        return FreeSql.Ado.Query<Navigation>($"select n.ID, n.ParentId, n.Name, n.[order], n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = @Category and d.Define = @Define left join Navigations ln on n.ParentId = ln.ID inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID ORDER BY n.Application, n.[order]", new { UserName = userName, Category = "菜单", RoleName = "Administrators", Define = EnumDictDefine.System });
    }

    public List<string> GetMenusByRoleId(string? roleId) => FreeSql.Ado.Query<string>("select NavigationID from NavigationRole where RoleID = @roleId", new { roleId });

    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from NavigationRole where RoleID = @roleId", new { roleId });
                FreeSql.Insert(menuIds.Select(g => new NavigationRole { NavigationID = g, RoleID = roleId })).ExecuteAffrows();
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }
}
