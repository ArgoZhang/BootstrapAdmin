using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class LoginService : ILogin
{
    public Task<bool> Log(string userName, bool result)
    {
        return Task.FromResult(true);
    }
}
