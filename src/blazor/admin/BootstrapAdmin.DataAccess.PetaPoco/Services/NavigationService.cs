// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System;
using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

/// <summary>
/// 
/// </summary>
class NavigationService : INavigation
{
    private IDatabase Database { get; }

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
        return CacheManager.GetOrAdd($"{nameof(NavigationService)}-{nameof(GetAllMenus)}-{userName}", entry =>
        {
            var order = Database.Provider.EscapeSqlIdentifier("Order");
            return Database.Fetch<Models.Navigation>($"select n.ID, n.ParentId, n.Name, n.{order}, n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application from Navigations n inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID ORDER BY n.Application, n.{order}", new { UserName = userName, RoleName = "Administrators" });
        });
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

            // 通知缓存更新
            ret = true;
            CacheManager.Clear($"{nameof(NavigationService)}-{nameof(GetAllMenus)}-*");
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }
        return ret;
    }
}
