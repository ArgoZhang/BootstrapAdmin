// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class RoleService : IRole
{
    private ISqlSugarClient db { get; }

    public RoleService(ISqlSugarClient db) => this.db = db;

    public List<Role> GetAll()
    {
        return db.Queryable<Role>().ToList();
    }

    public List<string> GetRolesByGroupId(string? groupId) =>
        //db.Ado.SqlQuery<string>("select RoleID from RoleGroup where GroupID = @groupId", new { groupId });
        db.Queryable<RoleGroup>().Where(t => t.GroupID == groupId).Select(t => t.RoleID).ToList();

    public List<string> GetRolesByMenuId(string? menuId) =>
        //db.Ado.SqlQuery<string>("select RoleID from NavigationRole where NavigationID = @menuId", new { menuId });
        db.Queryable<NavigationRole>().Where(t => t.NavigationID == menuId).Select(t => t.RoleID).ToList();

    public List<string> GetRolesByUserId(string? userId) =>
        //db.Ado.SqlQuery<string>("select RoleID from UserRole where UserID = @userId", new { userId });
        db.Queryable<UserRole>().Where(t => t.UserID == userId).Select(t => t.RoleID).ToList();


    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            //db.Ado.ExecuteCommand("delete from RoleGroup where GroupID = @groupId", new { groupId });
            db.Deleteable<RoleGroup>().Where(t => t.GroupID == groupId).ExecuteCommand();
            db.Insertable<RoleGroup>(roleIds.Select(g => new RoleGroup { RoleID = g, GroupID = groupId })).ExecuteCommand();
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

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            //db.Ado.ExecuteCommand("delete from NavigationRole where NavigationID = @menuId", new { menuId });
            db.Deleteable<NavigationRole>().Where(t => t.NavigationID == menuId).ExecuteCommand();
            db.Insertable<NavigationRole>(roleIds.Select(g => new NavigationRole { RoleID = g, NavigationID = menuId })).ExecuteCommand();
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

    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            //db.Ado.ExecuteCommand("delete from UserRole where UserID = @userId", new { userId });
            db.Deleteable<UserRole>().Where(t => t.UserID == userId).ExecuteCommand();
            db.Insertable<UserRole>(roleIds.Select(g => new UserRole { RoleID = g, UserID = userId })).ExecuteCommand();
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
