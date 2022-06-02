// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class RoleService : IRole
{
    private const string RoleServiceGetAllCacheKey = "RoleService-GetAll";

    private const string RoleServiceGetRolesByUserIdCacheKey = "RoleService-GetRolesByUserId";

    private const string RoleServiceGetRolesByGroupIdCacheKey = "RoleService-GetRolesByGroupId";

    private const string RoleServiceGetRolesByMenuIdCacheKey = "RoleService-GetRolesByMenusId";

    private IDBManager DBManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public RoleService(IDBManager db)
    {
        DBManager = db;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Role> GetAll() => CacheManager.GetOrAdd(RoleServiceGetAllCacheKey, entry => CacheManager.GetOrAdd(RoleServiceGetAllCacheKey, entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<Role>();
    }));

    public List<string> GetRolesByGroupId(string? groupId) => CacheManager.GetOrAdd($"{RoleServiceGetRolesByGroupIdCacheKey}-{groupId}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select RoleID from RoleGroup where GroupID = @0", groupId);
    });

    public List<string> GetRolesByUserId(string? userId) => CacheManager.GetOrAdd($"{RoleServiceGetRolesByUserIdCacheKey}-{userId}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select RoleID from UserRole where UserID = @0", userId);
    });

    public List<string> GetRolesByMenuId(string? menuId) => CacheManager.GetOrAdd($"{RoleServiceGetRolesByMenuIdCacheKey}-{menuId}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select RoleID from NavigationRole where NavigationID = @0", menuId);
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from RoleGroup where GroupID = @0", groupId);
            db.InsertBatch("RoleGroup", roleIds.Select(g => new { RoleID = g, GroupID = groupId }));
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from UserRole where UserID = @0", userId);
            db.InsertBatch("UserRole", roleIds.Select(g => new { RoleID = g, UserID = userId }));
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

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from NavigationRole where NavigationID = @0", menuId);
            db.InsertBatch("NavigationRole", roleIds.Select(g => new { RoleID = g, NavigationID = menuId }));
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
