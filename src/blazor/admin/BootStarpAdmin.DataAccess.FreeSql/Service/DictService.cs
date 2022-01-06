using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class DictService : IDict
{
    private IFreeSql FreeSql;

    public DictService(IFreeSql freeSql) => FreeSql = freeSql;


    public bool AuthenticateDemo(string code)
    {
        throw new NotImplementedException();
    }

    public List<Dict> GetAll()
    {
        return FreeSql.Select<Dict>().ToList();
    }

    public Dictionary<string, string> GetApps()
    {
        throw new NotImplementedException();
    }

    public int GetCookieExpiresPeriod()
    {
        throw new NotImplementedException();
    }

    public string GetCurrentLogin()
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetLogins()
    {
        throw new NotImplementedException();
    }

    public string? GetNotificationUrl(string appId)
    {
        throw new NotImplementedException();
    }

    public string? GetProfileUrl(string appId)
    {
        throw new NotImplementedException();
    }

    public string? GetSettingsUrl(string appId)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetThemes()
    {
        throw new NotImplementedException();
    }

    public string GetWebFooter() => "Footer";

    public string GetWebTitle() => "Title";

    public bool IsDemo()
    {
        throw new NotImplementedException();
    }

    public string RetrieveIconFolderPath()
    {
        throw new NotImplementedException();
    }

    public bool SaveCookieExpiresPeriod(int expiresPeriod)
    {
        throw new NotImplementedException();
    }

    public bool SaveDemo(bool isDemo)
    {
        throw new NotImplementedException();
    }

    public bool SaveHealthCheck(bool enable = true)
    {
        return true;
    }

    public bool SaveLogin(string login)
    {
        throw new NotImplementedException();
    }

    public bool SaveTheme(string theme)
    {
        throw new NotImplementedException();
    }

    public bool SaveWebFooter(string footer)
    {
        throw new NotImplementedException();
    }

    public bool SaveWebTitle(string title)
    {
        throw new NotImplementedException();
    }
}
