using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// Dict 字典表接口
/// </summary>
public interface IDict
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Dict> GetAll();

    /// <summary>
    /// 获得 配置所有的 App 集合
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetApps();

    /// <summary>
    /// 获得 配置所有的登录页集合
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetLogins();

    /// <summary>
    /// 获得 当前配置登录页
    /// </summary>
    /// <returns></returns>
    string GetCurrentLogin();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    bool SaveLogin(string login);

    /// <summary>
    /// 获得 配置所有的主题集合
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetThemes();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="theme"></param>
    /// <returns></returns>
    bool SaveTheme(string theme);

    /// <summary>
    /// 获取当前系统配置是否为演示模式
    /// </summary>
    /// <returns></returns>
    bool IsDemo();

    /// <summary>
    /// 保存当前网站是否为演示系统
    /// </summary>
    /// <param name="isDemo"></param>
    /// <returns></returns>
    bool SaveDemo(bool isDemo);

    /// <summary>
    /// 保存是否开启默认应用设置
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    bool SavDefaultApp(bool enabled);

    /// <summary>
    /// 保存健康检查
    /// </summary>
    /// <returns></returns>
    bool SaveHealthCheck(bool enable = true);

    /// <summary>
    /// 获得当前授权码是否有效可更改网站设置
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    bool AuthenticateDemo(string code);

    /// <summary>
    /// 获取 站点 Title 配置信息
    /// </summary>
    /// <returns></returns>
    string GetWebTitle();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    bool SaveWebTitle(string title);

    /// <summary>
    /// 获取站点 Footer 配置信息
    /// </summary>
    /// <returns></returns>
    string GetWebFooter();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="footer"></param>
    /// <returns></returns>
    bool SaveWebFooter(string footer);

    /// <summary>
    /// 获得 Cookie 登录持久化时长
    /// </summary>
    /// <returns></returns>
    int GetCookieExpiresPeriod();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expiresPeriod"></param>
    /// <returns></returns>
    bool SaveCookieExpiresPeriod(int expiresPeriod);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetProfileUrl(string appId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetSettingsUrl(string appId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetNotificationUrl(string appId);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string RetrieveIconFolderPath();

    /// <summary>
    /// 通过指定 appId 获得配置首页地址
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetHomeUrlByAppId(string? appId = null);

    /// <summary>
    /// 是否开启默认应用
    /// </summary>
    /// <returns></returns>
    bool GetEnableDefaultApp();
}
