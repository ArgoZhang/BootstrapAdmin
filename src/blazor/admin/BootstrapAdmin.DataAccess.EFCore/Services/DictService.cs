using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

class DictService : IDict
{
    public Dictionary<string, string> GetApps()
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetLogins()
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetThemes()
    {
        throw new NotImplementedException();
    }

    public string GetWebFooter()
    {
        throw new NotImplementedException();
    }

    public string GetWebTitle()
    {
        throw new NotImplementedException();
    }

    public bool IsDemo()
    {
        throw new NotImplementedException();
    }

    public bool SaveDemo(bool isDemo)
    {
        throw new NotImplementedException();
    }

    public bool AuthenticateDemo(string code)
    {
        throw new NotImplementedException();
    }

    public bool SaveHealthCheck(bool enable = true)
    {
        throw new NotImplementedException();
    }
}
