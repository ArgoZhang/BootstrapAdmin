// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.FreeSql.Service;

class GroupService : IGroup
{
    private IFreeSql FreeSql { get; }

    public GroupService(IFreeSql freeSql) => FreeSql = freeSql;

    public List<Group> GetAll() => FreeSql.Select<Group>().ToList();

    public List<string> GetGroupsByRoleId(string? roleId) => FreeSql.Ado.Query<string>("select GroupID from RoleGroup where RoleID = @roleId", new { roleId });

    public List<string> GetGroupsByUserId(string? userId) => FreeSql.Ado.Query<string>("select GroupID from UserGroup where UserID = @userId", new { userId });

    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from RoleGroup where RoleID = @roleId", new { roleId });
                FreeSql.Insert(groupIds.Select(g => new RoleGroup { GroupID = g, RoleID = roleId })).ExecuteAffrows();
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from UserGroup where UserID = @userId", new { userId });
                FreeSql.Insert(groupIds.Select(g => new UserGroup { GroupID = g, UserID = userId }));
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }
}
