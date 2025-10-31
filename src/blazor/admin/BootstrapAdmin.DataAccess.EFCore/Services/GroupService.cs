// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.EFCore.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

class GroupService(IDbContextFactory<BootstrapAdminContext> dbFactory) : IGroup, IDisposable
{
    private const string GroupServiceGetAllCacheKey = "GroupService-GetAll";

    private const string GroupServiceGetGroupsByUserIdCacheKey = "GroupService-GetGroupsByUserId";

    private const string GroupServiceGetGroupsByRoleIdCacheKey = "GroupService-GetGroupsByRoleId";

    private CancellationTokenSource? GetGroupsByUserIdCancellationTokenSource { get; set; }

    private CancellationTokenSource? GetGroupsByRoleIdCancellationTokenSource { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Group> GetAll()
    {
        using var context = dbFactory.CreateDbContext();

        return CacheManager.GetOrAdd(GroupServiceGetAllCacheKey, entry => context.Groups.AsNoTracking().ToList());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByRoleId(string? roleId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByRoleIdCacheKey}-{roleId}", entry =>
    {
        using var context = dbFactory.CreateDbContext();
        GetGroupsByRoleIdCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        var token = new CancellationChangeToken(GetGroupsByRoleIdCancellationTokenSource.Token);
        entry.ExpirationTokens.Add(token);
        return context.RoleGroup.Where(s => s.RoleId == roleId).Select(s => s.GroupId!).ToList();
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByUserId(string? userId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByUserIdCacheKey}-{userId}", entry =>
    {
        using var context = dbFactory.CreateDbContext();
        GetGroupsByUserIdCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        var token = new CancellationChangeToken(GetGroupsByUserIdCancellationTokenSource.Token);
        entry.ExpirationTokens.Add(token);
        return context.Set<UserGroup>().FromSqlRaw("select GroupID from UserGroup where UserID = {0}", userId!).Select(s => s.GroupId!).ToList();
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        using var context = dbFactory.CreateDbContext();
        var ret = false;
        try
        {
            context.Database.ExecuteSqlRaw("delete from RoleGroup where RoleID = {0}", roleId!);
            context.AddRange(groupIds.Select(g => new RoleGroup { GroupId = g, RoleId = roleId }));
            context.SaveChanges();
            ret = true;
        }
        catch (Exception)
        {
            throw;
        }

        if (ret)
        {
            // reset cache
            GetGroupsByRoleIdCancellationTokenSource?.Cancel();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        using var context = dbFactory.CreateDbContext();
        var ret = false;
        try
        {
            context.Database.ExecuteSqlRaw("delete from UserGroup where UserID = {0}", userId!);
            context.AddRange(groupIds.Select(g => new UserGroup { GroupId = g, UserId = userId }));
            context.SaveChanges();
            ret = true;
        }
        catch (Exception)
        {
            throw;
        }

        if (ret)
        {
            GetGroupsByUserIdCancellationTokenSource?.Cancel();
        }
        return ret;
    }
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            GetGroupsByRoleIdCancellationTokenSource?.Cancel();
            GetGroupsByRoleIdCancellationTokenSource?.Dispose();

            GetGroupsByUserIdCancellationTokenSource?.Cancel();
            GetGroupsByUserIdCancellationTokenSource?.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
