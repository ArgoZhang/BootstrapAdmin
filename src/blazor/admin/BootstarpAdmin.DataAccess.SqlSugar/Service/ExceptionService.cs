// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class ExceptionService(ISqlSugarClient db) : IException
{
    public (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, ExceptionFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        int count = 0;
        var errs = db.Queryable<Error>()
            .WhereIF(!string.IsNullOrEmpty(searchText), t => t.ErrorPage!.Contains(searchText!) || t.Message!.Contains(searchText!) || t.StackTrace!.Contains(searchText!))
            .WhereIF(!string.IsNullOrEmpty(filter.Category), t => t.Category == filter.Category)
            .WhereIF(!string.IsNullOrEmpty(filter.UserId), t => t.UserId!.Contains(filter.UserId!))
            .WhereIF(!string.IsNullOrEmpty(filter.ErrorPage), t => t.ErrorPage!.Contains(filter.ErrorPage!))
            .Where(t => t.LogTime >= filter.Star && t.LogTime <= filter.End)
            .OrderByIF(sortList.Any(), string.Join(", ", sortList))
            .OrderByIF(!sortList.Any(), "UserId, ErrorPage, LogTime desc")
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
