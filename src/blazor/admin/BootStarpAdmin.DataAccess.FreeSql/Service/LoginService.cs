using BootstrapAdmin.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class LoginService : ILogin
{
    public Task<bool> Log(string userName, bool result)
    {
        return Task.FromResult(true);
    }
}
