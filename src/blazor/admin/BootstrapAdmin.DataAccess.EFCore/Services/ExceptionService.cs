// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

/// <summary>
/// 
/// </summary>
public class ExceptionService : IException
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbFactory"></param>
    public ExceptionService(IDbContextFactory<BootstrapAdminContext> dbFactory) => DbFactory = dbFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="filter"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageItems"></param>
    /// <param name="sortList"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, ExceptionFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        var items = dbcontext.Set<Error>();

        if (!string.IsNullOrEmpty(searchText))
        {
            items.Where(s => s.Message!.Contains(searchText) || s.StackTrace!.Contains(searchText) || s.ErrorPage!.Contains(searchText));
        }

        if (!string.IsNullOrEmpty(filter.Category))
        {
            items.Where(s => s.Category!.Contains(filter.Category));
        }

        if (!string.IsNullOrEmpty(filter.UserId))
        {
            items.Where(s => s.UserId!.Contains(filter.UserId));
        }

        if (!string.IsNullOrEmpty(filter.ErrorPage))
        {
            items.Where(s => s.ErrorPage!.Contains(filter.ErrorPage));
        }

        items.Where(s => s.LogTime >= filter.Star && s.LogTime <= filter.End);

        if (sortList.Any())
        {
            items.Sort(sortList);
        }
        else
        {
            items.OrderByDescending(s => s.LogTime);
        }

        var data = items.Take(pageItems).Skip(pageItems * (pageIndex - 1)).AsNoTracking().ToList();
        return (data, items.Count());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Log(Error exception)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        dbcontext.Add(exception);
        return dbcontext.SaveChanges() > 0;
    }
}
