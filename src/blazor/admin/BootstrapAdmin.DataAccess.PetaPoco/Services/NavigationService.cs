// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

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
    private const string NavigationServiceGetAllCacheKey = "NavigationService-GetAll";

    private const string NavigationServiceGetMenusByRoleIdCacheKey = "NavigationService-GetMenusByRoleId";

    private IDBManager DBManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public NavigationService(IDBManager db)
    {
        DBManager = db;
    }

    /// <summary>
    /// 获得指定用户名可访问的所有菜单集合
    /// </summary>
    /// <param name="userName">当前用户名</param>
    /// <returns>未层次化的菜单集合</returns>
    public List<Navigation> GetAllMenus(string userName) => CacheManager.GetOrAdd($"{NavigationServiceGetAllCacheKey}-{userName}", entry =>
    {
        using var db = DBManager.Create();
        // 缓存所有菜单数据移除 SQL 语句降低复杂度
        var order = db.Provider.EscapeSqlIdentifier("Order");
        return db.Fetch<Models.Navigation>($"select n.ID, n.ParentId, n.Name, n.{order}, n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application from Navigations n inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID ORDER BY n.Application, n.{order}", new { UserName = userName, RoleName = "Administrators" });
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetMenusByRoleId(string? roleId) => CacheManager.GetOrAdd($"{NavigationServiceGetMenusByRoleIdCacheKey}-{roleId}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select NavigationID from NavigationRole where RoleID = @0", roleId);
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="menuIds"></param>
    /// <returns></returns>
    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from NavigationRole where RoleID = @0", roleId);
            db.InsertBatch("NavigationRole", menuIds.Select(g => new { NavigationID = g, RoleID = roleId }));
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
