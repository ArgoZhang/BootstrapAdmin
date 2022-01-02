using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.PetaPoco.Extensions;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class ExceptionService : BaseDatabase, IException
{
    public ExceptionService(IDatabase db) => Database = db;

    public bool Log(Error exception)
    {
        try
        {
            Database.Insert(exception);
        }
        catch { }
        return true;
    }

    public (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, ExceptionFilter filter, int pageIndex, int pageItems, string? sortName, string sortOrder)
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

        if (sortOrder == "Unset")
        {
            sortOrder = "desc";
        }

        if (string.IsNullOrEmpty(sortName))
        {
            sql.OrderBy("Logtime desc", "ErrorPage", "UserId");
        }
        else
        {
            sql.OrderBy($"{sortName} {sortOrder}");
        }

        var data = Database.Page<Error>(pageIndex, pageItems, sql);
        return (data.Items, data.TotalItems.ToInt32());
    }
}
