// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

/// <summary>
/// 
/// </summary>
public class TraceService : ITrace
{
    private ISqlSugarClient db { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public TraceService(ISqlSugarClient db) => this.db = db;

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
        //var items = db.Queryable<Trace>();
        //if (!string.IsNullOrEmpty(searchText))
        //{
        //    items.Where("UserName Like @searchText or Ip Like @searchText or RequestUrl Like @searchText", new { searchText });
        //}

        //if (!string.IsNullOrEmpty(filter.UserName))
        //{
        //    items.Where("UserName Like @UserName", new { filter.UserName });
        //}

        //if (!string.IsNullOrEmpty(filter.Ip))
        //{
        //    items.Where("Ip Like @Ip", new { filter.Ip });
        //}

        //if (!string.IsNullOrEmpty(filter.RequestUrl))
        //{
        //    items.Where("ErrorPage Like @RequestUrl", new { filter.RequestUrl });
        //}

        //items.Where("LogTime >= @Star and LogTime <= @End", new { filter.Star, filter.End });

        //if (sortList.Any())
        //{
        //    items.OrderBy(string.Join(", ", sortList));
        //}
        //else
        //{
        //    items.OrderBy("Logtime desc");
        //}
        //int count = 0;
        //var traces = items.ToPageList(pageIndex, pageItems, ref count);




        int count = 0;
        var traces = db.Queryable<Trace>()
            .WhereIF(!string.IsNullOrEmpty(searchText), t => t.UserName.Contains(searchText) || t.Ip.Contains(searchText) || t.RequestUrl.Contains(searchText))
            .WhereIF(!string.IsNullOrEmpty(filter.UserName), t => t.UserName == filter.UserName)
            .WhereIF(!string.IsNullOrEmpty(filter.Ip), t => t.Ip == filter.Ip)
            .WhereIF(!string.IsNullOrEmpty(filter.RequestUrl), t => t.RequestUrl == filter.RequestUrl)
            .Where(t => t.LogTime >= filter.Star && t.LogTime <= filter.End)
            .OrderByIF(sortList.Any(), string.Join(", ", sortList))
            .OrderByIF(!sortList.Any(), " Logtime desc")
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
