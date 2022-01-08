using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

class LoginService : ILogin
{
    public Task<bool> Log(string userName, bool result)
    {
        return Task.FromResult(true);
    }
}
