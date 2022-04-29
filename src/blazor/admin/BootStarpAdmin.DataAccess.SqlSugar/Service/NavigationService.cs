// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootStarpAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using SqlSugar;

namespace BootStarpAdmin.DataAccess.SqlSugar.Service;

class NavigationService : INavigation
{
    private ISqlSugarClient Client { get; }

    public NavigationService(ISqlSugarClient client) => Client = client;

    public List<Navigation> GetAllMenus(string userName)
    {
        return Client.Ado.SqlQuery<Navigation>($"select n.ID, n.ParentId, n.Name, n.[order], n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = @Category and d.Define = @Define left join Navigations ln on n.ParentId = ln.ID inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID ORDER BY n.Application, n.[order]", new { UserName = userName, Category = "菜单", RoleName = "Administrators", Define = EnumDictDefine.System });
    }

    public List<string> GetMenusByRoleId(string? roleId) => Client.Ado.SqlQuery<string>("select NavigationID from NavigationRole where RoleID = @RoleId", new { RoleId = roleId });

    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        var ret = false;
        try
        {
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from NavigationRole where RoleID = @RoleId", new { RoleId = roleId });
            Client.Insertable<NavigationRole>(menuIds.Select(g => new NavigationRole { NavigationID = g, RoleID = roleId })).ExecuteCommand();
            ret = true;
            Client.Ado.CommitTran();
        }
        catch (Exception)
        {
            Client.Ado.RollbackTran();
            throw;
        }
        return ret;
    }
}
