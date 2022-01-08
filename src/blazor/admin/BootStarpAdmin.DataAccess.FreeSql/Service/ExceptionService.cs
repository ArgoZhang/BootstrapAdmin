using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

class ExceptionService : IException
{
    private IFreeSql FreeSql { get; }

    public ExceptionService(IFreeSql freeSql) => FreeSql = freeSql;

    public (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, ExceptionFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        var items = FreeSql.Select<Error>();

        if (!string.IsNullOrEmpty(searchText))
        {
            items.Where($"ErrorPage Like %@searchText% or Message Like  %@searchText% or StackTrace Like  %@searchText%", new { searchText });
        }

        if (!string.IsNullOrEmpty(filter.Category))
        {
            items.Where("Category = @Category", new { filter.Category });
        }

        if (!string.IsNullOrEmpty(filter.UserId))
        {
            items.Where("UserId Like %@UserId%", new { filter.UserId });
        }

        if (!string.IsNullOrEmpty(filter.ErrorPage))
        {
            items.Where("ErrorPage Like %{ErrorPage}%", new { filter.ErrorPage });
        }

        items.Where("LogTime >= @Star and LogTime <= @End", new { filter.Star, filter.End });

        if (sortList.Any())
        {
            items.OrderBy(string.Join(", ", sortList));
        }
        else
        {
            items.OrderBy("UserId, ErrorPage, Logtime desc");
        }
        var errors = items.Count(out var count).Page(pageIndex, pageItems).ToList();

        return (errors, Convert.ToInt32(count));
    }

    public bool Log(Error exception)
    {
        try
        {
            FreeSql.Insert(exception).ExecuteAffrows();
        }
        catch { }
        return true;
    }
}
