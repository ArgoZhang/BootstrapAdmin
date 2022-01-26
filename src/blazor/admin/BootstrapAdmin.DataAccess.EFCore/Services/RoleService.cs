// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.EFCore.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

/// <summary>
/// 
/// </summary>
public class RoleService : IRole
{
    private const string RoleServiceGetAllCacheKey = "RoleService-GetAll";

    private const string RoleServiceGetRolesByUserIdCacheKey = "RoleService-GetRolesByUserId";

    private const string RoleServiceGetRolesByGroupIdCacheKey = "RoleService-GetRolesByGroupId";

    private CancellationTokenSource? GetRolesByUserIdCancellationTokenSource { get; set; }

    private CancellationTokenSource? GetRolesByGroupIdCancellationTokenSource { get; set; }

    private IDbContextFactory<BootstrapAdminContext> DbFactory { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbFactory"></param>
    public RoleService(IDbContextFactory<BootstrapAdminContext> dbFactory) => DbFactory = dbFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Role> GetAll()
    {
        return CacheManager.GetOrAdd(RoleServiceGetAllCacheKey, entry =>
        {
            using var dbcontext = DbFactory.CreateDbContext();

            return dbcontext.Roles.ToList();
        });

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>

    public List<string> GetRolesByGroupId(string? groupId)
    {
        return CacheManager.GetOrAdd($"{RoleServiceGetRolesByGroupIdCacheKey}-{groupId}", entry =>
         {
             using var dbcontext = DbFactory.CreateDbContext();

             return dbcontext.RoleGroup.Where(s => s.GroupId == groupId).Select(s => s.RoleId!).AsNoTracking().ToList();
         });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    public List<string> GetRolesByMenuId(string? menuId)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.NavigationRole.Where(s => s.NavigationId == menuId).Select(s => s.RoleId!).AsNoTracking().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> GetRolesByUserId(string? userId)
    {
        return CacheManager.GetOrAdd($"{RoleServiceGetRolesByUserIdCacheKey}-{userId}", entry =>
        {
            using var dbcontext = DbFactory.CreateDbContext();

            return dbcontext.UserRole.Where(s => s.UserId == userId).Select(s => s.RoleId!).AsNoTracking().ToList();
        });


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var ret = false;
        try
        {
            dbcontext.Database.ExecuteSqlRaw("delete from RoleGroup where GroupID = {0}", groupId!);
            dbcontext.AddRange(roleIds.Select(g => new RoleGroup { RoleId = g, GroupId = groupId }));
            dbcontext.SaveChanges();
            ret = true;
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var ret = false;
        try
        {
            dbcontext.Database.ExecuteSqlRaw("delete from NavigationRole where NavigationID = {0}", menuId!);
            dbcontext.AddRange(roleIds.Select(g => new NavigationRole { RoleId = g, NavigationId = menuId }));
            dbcontext.SaveChanges();
            ret = true;
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var ret = false;
        try
        {
            dbcontext.Database.ExecuteSqlRaw("delete from UserRole where UserID = {0}", userId!);
            dbcontext.AddRange(roleIds.Select(g => new UserRole { RoleId = g, UserId = userId }));
            dbcontext.SaveChanges();
            ret = true;
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }
}
