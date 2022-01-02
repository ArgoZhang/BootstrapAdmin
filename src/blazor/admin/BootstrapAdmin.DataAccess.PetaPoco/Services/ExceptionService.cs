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

    public (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, int pageIndex, int pageItems, string? sortName, string sortOrder)
    {
        var sql = new Sql();

        if (!string.IsNullOrEmpty(searchText))
        {
            sql.Append("WHERE ErrorPage Like @0 or Message Like @0 or StackTrace Like @0", $"%{searchText}%");
        }

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
