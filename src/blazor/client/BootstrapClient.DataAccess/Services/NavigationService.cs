// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapClient.DataAccess.Models;
using BootstrapClient.Web.Core;
using PetaPoco;

namespace BootstrapClient.DataAccess.PetaPoco.Services;

/// <summary>
/// 
/// </summary>
class NavigationService : INavigation
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public NavigationService(IDBManager db) => DBManager = db;

    /// <summary>
    /// 获得指定用户名可访问的所有菜单集合
    /// </summary>
    /// <param name="userName">当前用户名</param>
    /// <returns>未层次化的菜单集合</returns>
    public List<Navigation> GetMenus(string userName)
    {
        using var db = DBManager.Create ();
        var order = db.Provider.EscapeSqlIdentifier("Order");
        return db.Fetch<Navigation>($"select n.ID, n.ParentId, n.Name, n.{order}, n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application from Navigations n inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = 'Administrators')) nav on n.ID = nav.NavigationID Where n.Category = '1' ORDER BY n.Application, n.{order}", new { UserName = userName });
    }
}
