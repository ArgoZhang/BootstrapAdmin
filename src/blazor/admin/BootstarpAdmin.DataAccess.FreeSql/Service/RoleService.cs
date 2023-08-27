// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.FreeSql.Service;

class RoleService : IRole
{
    private IFreeSql FreeSql { get; }

    public RoleService(IFreeSql freeSql) => FreeSql = freeSql;

    public List<Role> GetAll()
    {
        return FreeSql.Select<Role>().ToList();
    }

    public List<string> GetRolesByGroupId(string? groupId) => FreeSql.Ado.Query<string>("select RoleID from RoleGroup where GroupID = @groupId", new { groupId });

    public List<string> GetRolesByMenuId(string? menuId) => FreeSql.Ado.Query<string>("select RoleID from NavigationRole where NavigationID = @menuId", new { menuId });

    public List<string> GetRolesByUserId(string? userId) => FreeSql.Ado.Query<string>("select RoleID from UserRole where UserID = @userId", new { userId });

    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from RoleGroup where GroupID = @groupId", new { groupId });
                FreeSql.Insert(roleIds.Select(g => new RoleGroup { RoleID = g, GroupID = groupId })).ExecuteAffrows();
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from NavigationRole where NavigationID = @menuId", new { menuId });
                FreeSql.Insert(roleIds.Select(g => new NavigationRole { RoleID = g, NavigationID = menuId })).ExecuteAffrows();
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from UserRole where UserID = @userId", new { userId });
                FreeSql.Insert(roleIds.Select(g => new UserRole { RoleID = g, UserID = userId })).ExecuteAffrows();
            });
            ret = true;
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }
}
