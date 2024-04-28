// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class NavigationService(ISqlSugarClient db) : INavigation
{
    public List<Navigation> GetAllMenus(string userName)
    {
        return db.Ado.SqlQuery<Navigation>($"select n.ID, n.ParentId, n.Name, n.[order], n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application from Navigations n inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID ORDER BY n.Application, n.[order]", new { UserName = userName, RoleName = "Administrators" });
    }

    public List<string> GetMenusByRoleId(string? roleId) => db.Queryable<NavigationRole>().Where(t => t.RoleID == roleId).Select(t => t.NavigationID!).ToList();

    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<NavigationRole>().Where(t => t.RoleID == roleId).ExecuteCommand();
            db.Insertable(menuIds.Select(g => new NavigationRole { NavigationID = g, RoleID = roleId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }
}
