// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootStarpAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.Caching;
using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

class AppService : IApp
{
    private const string AppServiceGetAppsByRoleIdCacheKey = "AppService-GetAppsByRoleId";

    private IFreeSql FreeSql { get; }

    public AppService(IFreeSql freeSql) => FreeSql = freeSql;

    public List<string> GetAppsByRoleId(string? roleId) => CacheManager.GetOrAdd($"{AppServiceGetAppsByRoleIdCacheKey}-{roleId}", entry => FreeSql.Ado.Query<string>("select AppID from RoleApp where RoleID = @roleId", new { roleId }));

    public bool SaveAppsByRoleId(string? roleId, IEnumerable<string> appIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from RoleApp where RoleID = @roleId", new { roleId });
                FreeSql.Insert(appIds.Select(g => new RoleApp { AppID = g, RoleID = roleId })).ExecuteAffrows();
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
