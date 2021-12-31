using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// Dict 字典表接口
/// </summary>
public interface IDict
{
    /// <summary>
    /// 获得 所有数据方法
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
    /// 获得 配置所有的主题集合
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetThemes();

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
    /// 获取站点 Footer 配置信息
    /// </summary>
    /// <returns></returns>
    string GetWebFooter();
}
