// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class ExceptionService : IException
{
    private IDBManager DBManager { get; }

    public ExceptionService(IDBManager db) => DBManager = db;

    public bool Log(Error exception)
    {
        try
        {
            using var db = DBManager.Create();
            db.Insert(exception);
        }
        catch { }
        return true;
    }

    public (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, ExceptionFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        var sql = new Sql();

        if (!string.IsNullOrEmpty(searchText))
        {
            sql.Where("ErrorPage Like @0 or Message Like @0 or StackTrace Like @0", $"%{searchText}%");
        }

        if (!string.IsNullOrEmpty(filter.Category))
        {
            sql.Where("Category = @0", filter.Category);
        }

        if (!string.IsNullOrEmpty(filter.UserId))
        {
            sql.Where("UserId Like @0", $"%{filter.UserId}%");
        }

        if (!string.IsNullOrEmpty(filter.ErrorPage))
        {
            sql.Where("ErrorPage Like @0", $"%{filter.ErrorPage}%");
        }

        sql.Where("LogTime >= @0 and LogTime <= @1", filter.Star, filter.End);

        if (sortList.Any())
        {
            sql.OrderBy(string.Join(", ", sortList));
        }
        else
        {
            sql.OrderBy("Logtime desc", "ErrorPage", "UserId");
        }

        using var db = DBManager.Create();
        var data = db.Page<Error>(pageIndex, pageItems, sql);
        return (data.Items, Convert.ToInt32(data.TotalItems));
    }
}
