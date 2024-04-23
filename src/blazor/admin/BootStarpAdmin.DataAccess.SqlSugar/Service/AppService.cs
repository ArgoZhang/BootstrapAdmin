// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class AppService(ISqlSugarClient db) : IApp
{
    public List<string> GetAppsByRoleId(string? roleId) => db.Queryable<RoleApp>().Where(t => t.RoleID == roleId).Select(t => t.AppID!).ToList();

    public bool SaveAppsByRoleId(string? roleId, IEnumerable<string> appIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            db.Deleteable<RoleApp>().Where(t => t.RoleID == roleId).ExecuteCommand();
            db.Insertable<RoleApp>(appIds.Select(g => new RoleApp { AppID = g, RoleID = roleId })).ExecuteCommand();
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
