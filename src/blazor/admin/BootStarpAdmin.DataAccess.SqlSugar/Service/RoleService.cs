// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootStarpAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using SqlSugar;

namespace BootstrapAdmin.DataAccess.SqlSugar.Services;

class RoleService : IRole
{
    private const string RoleServiceGetAllCacheKey = "RoleService-GetAll";

    private const string RoleServiceGetRolesByUserIdCacheKey = "RoleService-GetRolesByUserId";

    private const string RoleServiceGetRolesByGroupIdCacheKey = "RoleService-GetRolesByGroupId";

    private const string RoleServiceGetRolesByMenuIdCacheKey = "RoleService-GetRolesByMenusId";

    private ISqlSugarClient Client { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    public RoleService(ISqlSugarClient client) => Client = client;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Role> GetAll() => CacheManager.GetOrAdd(RoleServiceGetAllCacheKey, entry => CacheManager.GetOrAdd(RoleServiceGetAllCacheKey, entry => Client.Queryable<Role>().AS("Roles").ToList()));

    public List<string> GetRolesByGroupId(string? groupId) => CacheManager.GetOrAdd($"{RoleServiceGetRolesByGroupIdCacheKey}-{groupId}", entry => Client.Ado.SqlQuery<string>("select RoleID from RoleGroup where GroupID = @GroupID", new { GroupID = groupId }));

    public List<string> GetRolesByUserId(string? userId) => CacheManager.GetOrAdd($"{RoleServiceGetRolesByUserIdCacheKey}-{userId}", entry => Client.Ado.SqlQuery<string>("select RoleID from UserRole where UserID = @UserID", new { UserID = userId }));

    public List<string> GetRolesByMenuId(string? menuId) => CacheManager.GetOrAdd($"{RoleServiceGetRolesByMenuIdCacheKey}-{menuId}", entry => Client.Ado.SqlQuery<string>("select RoleID from NavigationRole where NavigationID = @NavigationID", new { NavigationID = menuId }));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from RoleGroup where GroupID = @GroupID", new { GroupID = groupId });
            Client.Insertable<RoleGroup>(roleIds.Select(g => new { RoleID = g, GroupID = groupId })).ExecuteCommand();
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
    /// <param name="userId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from UserRole where UserID = @UserID", new { UserID = userId });
            Client.Insertable<UserRole>(roleIds.Select(g => new { RoleID = g, UserID = userId })).ExecuteCommand();
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

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from NavigationRole where NavigationID = @NavigationID", new { NavigationID = menuId });
            Client.Insertable<NavigationRole>(roleIds.Select(g => new { RoleID = g, NavigationID = menuId }));
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
