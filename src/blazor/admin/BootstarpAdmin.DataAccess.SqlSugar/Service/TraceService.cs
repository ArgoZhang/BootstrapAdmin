// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class TraceService(ISqlSugarClient db) : ITrace
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="filter"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageItems"></param>
    /// <param name="sortList"></param>
    /// <returns></returns>
    public (IEnumerable<Trace> Items, int ItemsCount) GetAll(string? searchText, TraceFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        int count = 0;
        var traces = db.Queryable<Trace>()
            .WhereIF(!string.IsNullOrEmpty(searchText), t => t.UserName!.Contains(searchText!) || t.Ip!.Contains(searchText!) || t.RequestUrl!.Contains(searchText!))
            .WhereIF(!string.IsNullOrEmpty(filter.UserName), t => t.UserName == filter.UserName)
            .WhereIF(!string.IsNullOrEmpty(filter.Ip), t => t.Ip == filter.Ip)
            .WhereIF(!string.IsNullOrEmpty(filter.RequestUrl), t => t.RequestUrl == filter.RequestUrl)
            .Where(t => t.LogTime >= filter.Star && t.LogTime <= filter.End)
            .OrderByIF(sortList.Any(), string.Join(", ", sortList))
            .OrderByIF(!sortList.Any(), " LogTime desc")
            .ToPageList(pageIndex, pageItems, ref count);

        return (traces, count);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trace"></param>
    public void Log(Trace trace)
    {
        db.Insertable(trace).ExecuteCommand();
    }
}
