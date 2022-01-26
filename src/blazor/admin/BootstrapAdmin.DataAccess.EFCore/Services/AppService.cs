// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.EFCore.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

/// <summary>
/// 
/// </summary>
public class AppService : IApp
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbFactory"></param>
    public AppService(IDbContextFactory<BootstrapAdminContext> dbFactory) => DbFactory = dbFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<string> GetAppsByRoleId(string? roleId)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.RoleApp.Where(s => s.RoleID == roleId).Select(s => s.AppID!).AsNoTracking().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="appIds"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveAppsByRoleId(string? roleId, IEnumerable<string> appIds)
    {
        var ret = false;
        try
        {
            using var dbcontext = DbFactory.CreateDbContext();
            dbcontext.Database.ExecuteSqlRaw("delete from RoleApp where RoleID = {0}", roleId!);
            dbcontext.AddRange(appIds.Select(g => new RoleApp { AppID = g, RoleID = roleId }));
            ret = dbcontext.SaveChanges() > 0;
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }
}
