// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;


namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class GroupService(ISqlSugarClient db) : IGroup
{
    public List<Group> GetAll() => db.Queryable<Group>().ToList();

    public List<string> GetGroupsByRoleId(string? roleId) => db.Queryable<RoleGroup>().Where(t => t.RoleID == roleId).Select(t => t.GroupID!).ToList();

    public List<string> GetGroupsByUserId(string? userId) => db.Queryable<UserGroup>().Where(t => t.UserID == userId).Select(t => t.GroupID!).ToList();

    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<RoleGroup>().Where(t => t.RoleID == roleId).ExecuteCommand();
            db.Insertable(groupIds.Select(g => new RoleGroup { GroupID = g, RoleID = roleId }).ToList()).ExecuteCommand();
            ret = true;
            db.Ado.CommitTran();
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<UserGroup>().Where(t => t.UserID == userId).ExecuteCommand();
            db.Insertable(groupIds.Select(g => new UserGroup { GroupID = g, UserID = userId }).ToList()).ExecuteCommand();
            db.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }
}
