// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootStarpAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using SqlSugar;

namespace BootstrapAdmin.DataAccess.SqlSugar.Services;

class GroupService : IGroup
{
    private const string GroupServiceGetAllCacheKey = "GroupService-GetAll";

    private const string GroupServiceGetGroupsByUserIdCacheKey = "GroupService-GetGroupsByUserId";

    private const string GroupServiceGetGroupsByRoleIdCacheKey = "GroupService-GetGroupsByRoleId";

    private ISqlSugarClient Client { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    public GroupService(ISqlSugarClient client) => Client = client;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Group> GetAll() => CacheManager.GetOrAdd(GroupServiceGetAllCacheKey, entry => Client.Queryable<Group>().AS("Groups").ToList());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByUserId(string? userId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByUserIdCacheKey}-{userId}", entry => Client.Ado.SqlQuery<string>("select GroupID from UserGroup where UserID = @UserID", new { UserID = userId }));

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
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from UserGroup where UserID = @UserID", new { UserID = userId });
            Client.Insertable<UserGroup>(groupIds.Select(g => new { GroupID = g, UserID = userId })).ExecuteCommand();
            Client.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            Client.Ado.RollbackTran();
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
    public List<string> GetGroupsByRoleId(string? roleId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByRoleIdCacheKey}-{roleId}", entry => Client.Ado.SqlQuery<string>("select GroupID from RoleGroup where RoleID = @RoleID", new { RoleID = roleId }));

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
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from RoleGroup where RoleID = @RoleID", new { RoleID = roleId });
            Client.Insertable<RoleGroup>(groupIds.Select(g => new { GroupID = g, RoleID = roleId }));
            Client.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            Client.Ado.RollbackTran();
            throw;
        }

        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
