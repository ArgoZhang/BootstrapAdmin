// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class AppService : IApp
{
    private const string AppServiceGetAppsByRoleIdCacheKey = "AppService-GetAppsByRoleId";

    private IDBManager DBManager { get; }

    public AppService(IDBManager db)
    {
        DBManager = db;
    }

    public List<string> GetAppsByRoleId(string? roleId) => CacheManager.GetOrAdd($"{AppServiceGetAppsByRoleIdCacheKey}-{roleId}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select AppID from RoleApp where RoleID = @0", roleId);
    });

    public bool SaveAppsByRoleId(string? roleId, IEnumerable<string> appIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from RoleApp where RoleID = @0", roleId);
            db.InsertBatch("RoleApp", appIds.Select(g => new { AppID = g, RoleID = roleId }));
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
