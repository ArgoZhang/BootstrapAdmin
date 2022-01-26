// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class GroupService : IGroup
{
    private const string GroupServiceGetAllCacheKey = "GroupService-GetAll";

    private const string GroupServiceGetGroupsByUserIdCacheKey = "GroupService-GetGroupsByUserId";

    private const string GroupServiceGetGroupsByRoleIdCacheKey = "GroupService-GetGroupsByRoleId";

    private IDatabase Database { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public GroupService(IDatabase db) => Database = db;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Group> GetAll() => CacheManager.GetOrAdd(GroupServiceGetAllCacheKey, entry => Database.Fetch<Group>());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByUserId(string? userId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByUserIdCacheKey}-{userId}", entry => Database.Fetch<string>("select GroupID from UserGroup where UserID = @0", userId));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from UserGroup where UserID = @0", userId);
            Database.InsertBatch("UserGroup", groupIds.Select(g => new { GroupID = g, UserID = userId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
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
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByRoleId(string? roleId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByRoleIdCacheKey}-{roleId}", entry => Database.Fetch<string>("select GroupID from RoleGroup where RoleID = @0", roleId));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from RoleGroup where RoleID = @0", roleId);
            Database.InsertBatch("RoleGroup", groupIds.Select(g => new { GroupID = g, RoleID = roleId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }

        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
