// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

class TraceService(IDbContextFactory<BootstrapAdminContext> dbFactory) : ITrace
{
    public (IEnumerable<Trace> Items, int ItemsCount) GetAll(string? searchText, TraceFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        using var context = dbFactory.CreateDbContext();

        var items = context.Set<Trace>();

        if (!string.IsNullOrEmpty(searchText))
        {
            items.Where(s => s.UserName!.Contains(searchText) || s.Ip!.Contains(searchText) || s.RequestUrl!.Contains(searchText));
        }

        if (!string.IsNullOrEmpty(filter.UserName))
        {
            items.Where(s => s.UserName!.Contains(filter.UserName));
        }

        if (!string.IsNullOrEmpty(filter.Ip))
        {
            items.Where(s => s.Ip!.Contains(filter.Ip));
        }

        if (!string.IsNullOrEmpty(filter.RequestUrl))
        {
            items.Where(s => s.RequestUrl!.Contains(filter.RequestUrl));
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
    /// <param name="trace"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Log(Trace trace)
    {
        using var context = dbFactory.CreateDbContext();

        context.Add(trace);
        context.SaveChanges();
    }
}
