using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

public interface IException
{
    bool Log(Error exception);

    (IEnumerable<Error> Items, int ItemsCount) GetAll(string? searchText, ExceptionFilter filter, int pageIndex, int pageItems, string? sortName, string sortOrder);
}
