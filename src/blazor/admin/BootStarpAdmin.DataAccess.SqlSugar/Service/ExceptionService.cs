// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;


namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class ExceptionService : IException
{
    private ISqlSugarClient db { get; }

    public ExceptionService(ISqlSugarClient db) => this.db = db;

    public (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, ExceptionFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {

        ////原本写法
        //var items = db.Queryable<Error>();

        //if (!string.IsNullOrEmpty(searchText))
        //{
        //    items.Where($"ErrorPage Like %@searchText% or Message Like  %@searchText% or StackTrace Like  %@searchText%", new { searchText });
        //}

        //if (!string.IsNullOrEmpty(filter.Category))
        //{
        //    items.Where("Category = @Category", new { filter.Category });
        //}

        //if (!string.IsNullOrEmpty(filter.UserId))
        //{
        //    items.Where("UserId Like %@UserId%", new { filter.UserId });
        //}

        //if (!string.IsNullOrEmpty(filter.ErrorPage))
        //{
        //    items.Where("ErrorPage Like %{ErrorPage}%", new { filter.ErrorPage });
        //}

        //items.Where("LogTime >= @Star and LogTime <= @End", new { filter.Star, filter.End });

        //if (sortList.Any())
        //{
        //    items.OrderBy(string.Join(", ", sortList));
        //}
        //else
        //{
        //    items.OrderBy("UserId, ErrorPage, Logtime desc");
        //}
        //int count = 0;
        //var errors = items.ToPageList(pageIndex, pageItems, ref count);




        //改linq写法 精简改写
        int count = 0;
        var errs = db.Queryable<Error>()
            .WhereIF(!string.IsNullOrEmpty(searchText), t => t.ErrorPage.Contains(searchText) || t.Message.Contains(searchText) || t.StackTrace.Contains(searchText))
            .WhereIF(!string.IsNullOrEmpty(filter.Category), t => t.Category == filter.Category)
            .WhereIF(!string.IsNullOrEmpty(filter.UserId), t => t.UserId.Contains(filter.UserId))
            .WhereIF(!string.IsNullOrEmpty(filter.ErrorPage), t => t.ErrorPage.Contains(filter.ErrorPage))
            .Where(t => t.LogTime >= filter.Star && t.LogTime <= filter.End)
            .OrderByIF(sortList.Any(), string.Join(", ", sortList))
            .OrderByIF(!sortList.Any(), "UserId, ErrorPage, Logtime desc")
            .ToPageList(pageIndex, pageItems, ref count);

        return (errs, count);
    }

    public bool Log(Error exception)
    {
        try
        {
            db.Insertable(exception).ExecuteCommand();
        }
        catch { }
        return true;
    }
}
