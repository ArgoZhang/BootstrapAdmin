// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;


namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class GroupService : IGroup
{
    private ISqlSugarClient db { get; }

    public GroupService(ISqlSugarClient db) => this.db = db;

    public List<Group> GetAll() => db.Queryable<Group>().ToList();

    public List<string> GetGroupsByRoleId(string? roleId) =>
        //db.Ado.SqlQuery<string>("select GroupID from RoleGroup where RoleID = @roleId", new { roleId });
        db.Queryable<RoleGroup>().Where(t => t.RoleID == roleId).Select(t => t.GroupID).ToList();
    public List<string> GetGroupsByUserId(string? userId) =>
        //db.Ado.SqlQuery<string>("select GroupID from UserGroup where UserID = @userId", new { userId });
        db.Queryable<UserGroup>().Where(t => t.UserID == userId).Select(t => t.GroupID).ToList();
    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            //db.Ado.ExecuteCommand("delete from RoleGroup where RoleID = @roleId", new { roleId });
            db.Deleteable<RoleGroup>().Where(t => t.RoleID == roleId).ExecuteCommand();
            db.Insertable<RoleGroup>(groupIds.Select(g => new RoleGroup { GroupID = g, RoleID = roleId })).ExecuteCommand();
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
            //db.Ado.ExecuteCommand("delete from UserGroup where UserID = @userId", new { userId });
            db.Deleteable<UserGroup>().Where(t => t.UserID == userId).ExecuteCommand();
            db.Insertable<UserGroup>(groupIds.Select(g => new UserGroup { GroupID = g, UserID = userId })).ExecuteCommand();
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
}
