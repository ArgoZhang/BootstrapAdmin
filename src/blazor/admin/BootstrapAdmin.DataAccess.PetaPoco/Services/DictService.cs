using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Longbow.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class DictService : IDict
{
    private const string DictServiceCacheKey = "DictService-GetAll";

    private IDatabase Database { get; }

    private string AppId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    /// <param name="configuration"></param>
    public DictService(IDatabase db, IConfiguration configuration)
    {
        Database = db;
        AppId = configuration.GetValue("AppId", "BA");
    }

    public List<Dict> GetAll() => CacheManager.GetOrAdd(DictServiceCacheKey, entry => Database.Fetch<Dict>());

    public Dictionary<string, string> GetApps()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == "应用程序").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
    }

    public Dictionary<string, string> GetLogins()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == "系统首页").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
    }

    public string GetCurrentLogin()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "登录界面" && d.Define == EnumDictDefine.System)?.Code ?? "Login";
    }

    public Dictionary<string, string> GetThemes()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == "网站样式").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
    }

    /// <summary>
    /// 获取 站点 Title 配置信息
    /// </summary>
    /// <returns></returns>
    public string GetWebTitle()
    {
        var dicts = GetAll();
        var title = "网站标题";
        var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站标题") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站标题");
            title = dict?.Code ?? "网站标题";
        }
        return title;
    }

    /// <summary>
    /// 获取站点 Footer 配置信息
    /// </summary>
    /// <returns></returns>
    public string GetWebFooter()
    {
        var dicts = GetAll();
        var title = "网站页脚";
        var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站页脚") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站页脚");
            title = dict?.Code ?? "网站标题";
        }
        return title;
    }

    /// <summary>
    /// 获得 应用是否为演示模式
    /// </summary>
    /// <returns></returns>
    public bool IsDemo()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "演示系统" && d.Define == EnumDictDefine.System)?.Code ?? "0";
        return code == "1";
    }

    /// <summary>
    /// 获得当前授权码是否有效可更改网站设置
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public bool AuthenticateDemo(string code)
    {
        var ret = false;
        if (!string.IsNullOrEmpty(code))
        {
            var dicts = GetAll();
            var salt = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "授权盐值" && d.Define == EnumDictDefine.System)?.Code;
            var authCode = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "哈希结果" && d.Define == EnumDictDefine.System)?.Code;
            if (!string.IsNullOrEmpty(salt))
            {
                ret = LgbCryptography.ComputeHash(code, salt) == authCode;
            }
        }
        return ret;
    }

    /// <summary>
    /// 保存当前网站是否为演示系统
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    public bool SaveDemo(bool enable) => SaveDict(new Dict
    {
        Category = "网站设置",
        Name = "演示系统",
        Code = enable ? "1" : "0",
        Define = EnumDictDefine.System
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    public bool SavDefaultApp(bool enable) => SaveDict(new Dict
    {
        Category = "网站设置",
        Name = "默认应用程序",
        Code = enable ? "1" : "0",
        Define = EnumDictDefine.System
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveHealthCheck(bool enable = true) => SaveDict(new Dict
    {
        Category = "网站设置",
        Name = "健康检查",
        Code = enable ? "1" : "0",
        Define = EnumDictDefine.System
    });

    /// <summary>
    /// 获取当前网站 Cookie 保持时长
    /// </summary>
    /// <returns></returns>
    public int GetCookieExpiresPeriod()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "Cookie保留时长" && d.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(code, out var ret);
        return ret;
    }

    private bool SaveDict(Dict dict)
    {
        var ret = Database.Update<Dict>("set Code = @Code where Category = @Category and Name = @Name", dict) == 1;
        if (ret)
        {
            // 更新缓存
            CacheManager.Clear(DictServiceCacheKey);
        }
        return ret;
    }

    public bool SaveLogin(string login) => SaveDict(new Dict { Category = "网站设置", Name = "登录界面", Code = login });

    public bool SaveTheme(string theme) => SaveDict(new Dict { Category = "网站设置", Name = "使用样式", Code = theme });

    public bool SaveWebTitle(string title) => SaveDict(new Dict { Category = "网站设置", Name = "网站标题", Code = title });

    public bool SaveWebFooter(string footer) => SaveDict(new Dict { Category = "网站设置", Name = "网站页脚", Code = footer });

    public bool SaveCookieExpiresPeriod(int expiresPeriod) => SaveDict(new Dict { Category = "网站设置", Name = "Cookie保留时长", Code = expiresPeriod.ToString() });

    public string? GetProfileUrl(string appId) => GetUrlByName(appId, "个人中心地址");

    public string? GetSettingsUrl(string appId) => GetUrlByName(appId, "系统设置地址");

    public string? GetNotificationUrl(string appId) => GetUrlByName(appId, "系统通知地址");

    public bool GetEnableDefaultApp()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "默认应用程序")?.Code ?? "0";
        return code == "1";
    }

    private string? GetUrlByName(string appId, string dictName)
    {
        string? url = null;
        var dicts = GetAll();
        var appName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId && d.Define == EnumDictDefine.System)?.Name;
        if (!string.IsNullOrEmpty(appName))
        {
            url = dicts.FirstOrDefault(d => d.Category == appName && d.Name == dictName && d.Define == EnumDictDefine.Customer)?.Code;
        }
        return url;
    }

    /// <summary>
    /// 获取头像路径
    /// </summary>
    /// <returns></returns>
    public string GetIconFolderPath()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == EnumDictDefine.System)?.Code ?? "/images/uploder/";
    }

    /// <summary>
    /// 获取头像路径
    /// </summary>
    /// <returns></returns>
    public string GetDefaultIcon()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Name == "头像文件" && d.Category == "头像地址" && d.Define == EnumDictDefine.System)?.Code ?? "default.jpg";
    }

    /// <summary>
    /// 通过指定 appId 获得配置首页地址
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    public string? GetHomeUrlByAppId(string? appId = null)
    {
        string? url = null;
        var dicts = GetAll();

        // 查看是否开启默认应用
        var enableDefaultApp = GetEnableDefaultApp();
        if (enableDefaultApp)
        {
            // appId 为空时读取前台列表取第一个应用作为默认应用
            appId ??= GetApps().FirstOrDefault(d => d.Key != AppId).Key ?? AppId;
            url = dicts.FirstOrDefault(d => d.Category == "应用首页" && d.Name.Equals(appId, StringComparison.OrdinalIgnoreCase) && d.Define == EnumDictDefine.System)?.Code;
        }
        return url;
    }

    public bool GetAppSiderbar()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "侧边栏状态" && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppSiderbar(bool value) => SaveDict(new Dict { Category = "网站设置", Name = "侧边栏状态", Code = value ? "1" : "0" });

    public bool GetAppTitle()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "卡片标题状态" && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppTitle(bool value) => SaveDict(new Dict { Category = "网站设置", Name = "卡片标题状态", Code = value ? "1" : "0" });

    public bool GetAppFixHeader()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "固定表头" && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppFixHeader(bool value) => SaveDict(new Dict { Category = "网站设置", Name = "固定表头", Code = value ? "1" : "0" });

    public bool GetAppHealthCheck()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "健康检查" && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppHealthCheck(bool value) => SaveDict(new Dict { Category = "网站设置", Name = "健康检查", Code = value ? "1" : "0" });

    public bool GetAppMobileLogin()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "短信验证码登录" && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppMobileLogin(bool value) => SaveDict(new Dict { Category = "网站设置", Name = "短信验证码登录", Code = value ? "1" : "0" });

    public bool GetAppOAuthLogin()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "OAuth 认证登录" && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppOAuthLogin(bool value) => SaveDict(new Dict { Category = "网站设置", Name = "OAuth 认证登录", Code = value ? "1" : "0" });

    public bool GetAutoLockScreen()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "自动锁屏" && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAutoLockScreen(bool value) => SaveDict(new Dict { Category = "网站设置", Name = "自动锁屏", Code = value ? "1" : "0" });

    public int GetAutoLockScreenInterval()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "自动锁屏时长" && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveAutoLockScreenInterval(int value) => SaveDict(new Dict { Category = "网站设置", Name = "自动锁屏时长", Code = value.ToString() });

    public Dictionary<string, string> GetIps()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == "地理位置服务").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
    }

    public string? GetCurrentIp()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "IP地理位置接口" && s.Define == EnumDictDefine.System)?.Code;
    }

    public bool SaveCurrentIp(string value) => SaveDict(new Dict { Category = "网站设置", Name = "IP地理位置接口", Code = value });

    public int GetCookieExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "Cookie保留时长" && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveCookieExpired(int value) => SaveDict(new Dict { Category = "网站设置", Name = "Cookie保留时长", Code = value.ToString() });

    public int GetExceptionExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "程序异常保留时长" && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveExceptionExpired(int value) => SaveDict(new Dict { Category = "网站设置", Name = "程序异常保留时长", Code = value.ToString() });

    public int GetOperateExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "操作日志保留时长" && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveOperateExpired(int value) => SaveDict(new Dict { Category = "网站设置", Name = "操作日志保留时长", Code = value.ToString() });

    public int GetLoginExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "登录日志保留时长" && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveLoginExpired(int value) => SaveDict(new Dict { Category = "网站设置", Name = "登录日志保留时长", Code = value.ToString() });

    public int GetAccessExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "访问日志保留时长" && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveAccessExpired(int value) => SaveDict(new Dict { Category = "网站设置", Name = "访问日志保留时长", Code = value.ToString() });

    public int GetIPCacheExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == "网站设置" && s.Name == "IP请求缓存时长" && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveIPCacheExpired(int value) => SaveDict(new Dict { Category = "网站设置", Name = "IP请求缓存时长", Code = value.ToString() });

    public Dictionary<string, string> GetFrontApp()
    {
        var dicts = GetAll();
        return dicts.Where(s => s.Category == "应用程序" && s.Code != "BA").ToDictionary(s => s.Name, s => s.Code);
    }

    public string GetFrontUrl(string name)
    {
        var dicts = GetAll();
        return dicts.Where(s => s.Category == "应用首页" && s.Name == name).FirstOrDefault()?.Code ?? "";
    }

    public bool ExistsAppId(string appId)
    {
        var dicts = GetAll();
        return dicts.Exists(s => s.Category == "应用程序" && s.Code == appId);
    }

    public bool SaveFrontApp(string appId, string AppName, string homeUrl, string title, string footer, string icon, string favicon)
    {
        var items = new List<Dict>()
        {
            new Dict { Category = "应用程序", Name = AppName, Code = appId, Define = EnumDictDefine.System },
            new Dict { Category = AppName, Name = "网站页脚", Code = footer },
            new Dict { Category = AppName, Name = "网站标题", Code = title },
            new Dict { Category = AppName, Name = "favicon", Code = favicon },
            new Dict { Category = AppName, Name = "网站图标", Code = icon },
            new Dict { Category = "应用首页", Name = appId, Code = homeUrl },
        };
        var exist = ExistsAppId(appId);

        if (exist)
        {
            try
            {
                Database.BeginTransaction();
                Database.Execute("update Dicts set Code=@HomeUrl where Category=@Category and Name=@Name", new { HomeUrl = homeUrl, Category = AppName, Name = appId });
                Database.Execute("update Dicts set Code=@Footer where Category=@Category and Name='网站页脚'", new { Footer = footer, Category = AppName });
                Database.Execute("update Dicts set Code=@Title where Category=@Category and Name='网站标题'", new { Title = title, Category = AppName });
                Database.Execute("update Dicts set Code=@Favicon where Category=@Category and Name='favicon'", new { Favicon = favicon, Category = AppName });
                Database.Execute("update Dicts set Code=@Icon where Category=@Category and Name='网站图标'", new { Icon = icon, Category = AppName });
                Database.CompleteTransaction();
            }
            catch (Exception)
            {
                Database.AbortTransaction();
                throw;
            }
        }
        else
        {
            Database.InsertBatch(items);
        }
        return true;
    }

    public (string homeurl, string title, string footer, string icon, string favicon) GetFrontAppSettings(string appId, string AppName)
    {
        var dicts = GetAll();
        var homeurl = dicts.FirstOrDefault(s => s.Category == "应用首页" && s.Name == appId)?.Code ?? "";
        var title = dicts.FirstOrDefault(s => s.Category == AppName && s.Name == "网站标题")?.Code ?? "";
        var footer = dicts.FirstOrDefault(s => s.Category == AppName && s.Name == "网站页脚")?.Code ?? "";
        var icon = dicts.FirstOrDefault(s => s.Category == AppName && s.Name == "网站图标")?.Code ?? "";
        var favicon = dicts.FirstOrDefault(s => s.Category == AppName && s.Name == "favicon")?.Code ?? "";

        return (homeurl, title, footer, icon, favicon);
    }

    public bool DeleteFrontAppSettings(string appId, string AppName)
    {
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from dicts where Category='应用程序' and Name=@Name and Code=@Code", new { Name = AppName, Code = appId });
            Database.Execute("delete from dicts where Category=@Category and Name='网站页脚'", new { Category = AppName });
            Database.Execute("delete from dicts where Category=@Category and Name='网站页脚'", new { Category = AppName });
            Database.Execute("delete from dicts where Category=@Category and Name='网站标题'", new { Category = AppName });
            Database.Execute("delete from dicts where Category=@Category and Name='favicon'", new { Category = AppName });
            Database.Execute("delete from dicts where Category=@Category and Name='网站图标'", new { Category = AppName });
            Database.CompleteTransaction();
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }

        return true;
    }
}
