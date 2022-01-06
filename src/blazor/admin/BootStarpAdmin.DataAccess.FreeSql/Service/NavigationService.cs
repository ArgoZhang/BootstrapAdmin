using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class NavigationService : INavigation
{
    private IFreeSql FreeSql;

    public NavigationService(IFreeSql freeSql) => FreeSql = freeSql;

    public List<Navigation> GetAllMenus(string userName)
    {
        return FreeSql.Select<Navigation>().ToList();
    }

    public List<string> GetMenusByRoleId(string? roleId)
    {
        throw new NotImplementedException();
    }

    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        throw new NotImplementedException();
    }
}
