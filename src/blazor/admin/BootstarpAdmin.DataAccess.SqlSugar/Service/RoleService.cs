// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class RoleService(ISqlSugarClient db) : IRole
{
    public List<Role> GetAll()
    {
        return db.Queryable<Role>().ToList();
    }

    public List<string> GetRolesByGroupId(string? groupId) => db.Queryable<RoleGroup>().Where(t => t.GroupID == groupId).Select(t => t.RoleID!).ToList();

    public List<string> GetRolesByMenuId(string? menuId) => db.Queryable<NavigationRole>().Where(t => t.NavigationID == menuId).Select(t => t.RoleID!).ToList();

    public List<string> GetRolesByUserId(string? userId) => db.Queryable<UserRole>().Where(t => t.UserID == userId).Select(t => t.RoleID!).ToList();

    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<RoleGroup>().Where(t => t.GroupID == groupId).ExecuteCommand();
            db.Insertable(roleIds.Select(g => new RoleGroup { RoleID = g, GroupID = groupId }).ToList()).ExecuteCommand();
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

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<NavigationRole>().Where(t => t.NavigationID == menuId).ExecuteCommand();
            db.Insertable(roleIds.Select(g => new NavigationRole { RoleID = g, NavigationID = menuId }).ToList()).ExecuteCommand();
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

    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<UserRole>().Where(t => t.UserID == userId).ExecuteCommand();
            db.Insertable(roleIds.Select(g => new UserRole { RoleID = g, UserID = userId }).ToList()).ExecuteCommand();
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
