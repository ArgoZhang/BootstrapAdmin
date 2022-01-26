// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

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
    /// 
    /// </summary>
    /// <returns></returns>
    string? GetIpLocatorName();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    string? GetIpLocatorUrl(string? name);

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
    string GetIconFolderPath();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string GetDefaultIcon();

    /// <summary>
    /// 通过指定 appId 获得配置首页地址
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetHomeUrlByAppId(string appId);

    /// <summary>
    /// 是否开启默认应用
    /// </summary>
    /// <returns></returns>
    bool GetEnableDefaultApp();

    /// <summary>
    /// 是否开启侧边栏设置
    /// </summary>
    /// <returns></returns>
    bool GetAppSiderbar();

    /// <summary>
    /// 保存侧边栏设置
    /// </summary>
    /// <returns></returns>
    bool SaveAppSiderbar(bool value);

    /// <summary>
    /// 是否开启标题设置
    /// </summary>
    /// <returns></returns>
    bool GetAppTitle();

    /// <summary>
    /// 保存标题设置
    /// </summary>
    /// <returns></returns>
    bool SaveAppTitle(bool value);

    /// <summary>
    /// 是否开启固定表头设置
    /// </summary>
    /// <returns></returns>
    bool GetAppFixHeader();

    /// <summary>
    /// 保存固定表头设置
    /// </summary>
    /// <returns></returns>
    bool SaveAppFixHeader(bool value);

    /// <summary>
    /// 是否开启健康检查设置
    /// </summary>
    /// <returns></returns>
    bool GetAppHealthCheck();

    /// <summary>
    /// 保存健康检查设置
    /// </summary>
    /// <returns></returns>
    bool SaveAppHealthCheck(bool value);

    /// <summary>
    /// 是否开启手机认证设置
    /// </summary>
    /// <returns></returns>
    bool GetAppMobileLogin();

    /// <summary>
    /// 保存手机认证设置
    /// </summary>
    /// <returns></returns>
    bool SaveAppMobileLogin(bool value);

    /// <summary>
    /// 是否开启 OAuth 认证设置
    /// </summary>
    /// <returns></returns>
    bool GetAppOAuthLogin();

    /// <summary>
    /// 保存 OAuth 认证设置
    /// </summary>
    /// <returns></returns>
    bool SaveAppOAuthLogin(bool value);

    /// <summary>
    /// 是否开启自动锁屏设置
    /// </summary>
    /// <returns></returns>
    bool GetAutoLockScreen();

    /// <summary>
    /// 保存自动锁屏设置
    /// </summary>
    /// <returns></returns>
    bool SaveAutoLockScreen(bool value);

    /// <summary>
    /// 获得自动锁屏间隔时间
    /// </summary>
    /// <returns></returns>
    int GetAutoLockScreenInterval();

    /// <summary>
    /// 保存自动锁屏间隔时间
    /// </summary>
    /// <returns></returns>
    bool SaveAutoLockScreenInterval(int value);

    /// <summary>
    /// 获得地理位置服务
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetIpLocators();

    /// <summary>
    /// 获得当前地理位置服务
    /// </summary>
    /// <returns></returns>
    string? GetIpLocator();

    /// <summary>
    /// 设置当前地理位置服务
    /// </summary>
    /// <returns></returns>
    bool SaveCurrentIp(string value);

    /// <summary>
    /// 获得 Cookie 过期时间
    /// </summary>
    /// <returns></returns>
    int GetCookieExpired();

    /// <summary>
    /// 设置 Cookie 过期时间
    /// </summary>
    /// <returns></returns>
    bool SaveCookieExpired(int value);

    /// <summary>
    /// 获得程序异常保留时长
    /// </summary>
    /// <returns></returns>
    int GetExceptionExpired();

    /// <summary>
    /// 设置程序异常保留时长
    /// </summary>
    /// <returns></returns>
    bool SaveExceptionExpired(int value);

    /// <summary>
    /// 获得操作日志保留时长
    /// </summary>
    /// <returns></returns>
    int GetOperateExpired();

    /// <summary>
    /// 设置操作日志保留时长
    /// </summary>
    /// <returns></returns>
    bool SaveOperateExpired(int value);

    /// <summary>
    /// 获得登录日志保留时长
    /// </summary>
    /// <returns></returns>
    int GetLoginExpired();

    /// <summary>
    /// 设置登录日志保留时长
    /// </summary>
    /// <returns></returns>
    bool SaveLoginExpired(int value);

    /// <summary>
    /// 获得访问日志保留时长
    /// </summary>
    /// <returns></returns>
    int GetAccessExpired();

    /// <summary>
    /// 设置访问日志保留时长
    /// </summary>
    /// <returns></returns>
    bool SaveAccessExpired(int value);

    /// <summary>
    /// 获得 IP 请求缓存时长
    /// </summary>
    /// <returns></returns>
    int GetIPCacheExpired();

    /// <summary>
    /// 设置 IP 请求缓存时长
    /// </summary>
    /// <returns></returns>
    bool SaveIPCacheExpired(int value);

    /// <summary>
    /// 获得前台应用
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string>? GetClients();

    /// <summary>
    /// 获得前台应用
    /// </summary>
    /// <returns></returns>
    string GetClientUrl(string name);

    /// <summary>
    /// 获得前台应用
    /// </summary>
    /// <returns></returns>
    bool ExistsAppId(string appId);

    /// <summary>
    /// 添加前台应用
    /// </summary>
    /// <returns></returns>
    bool SaveClient(ClientApp client);

    /// <summary>
    /// 获得前台应用配置
    /// </summary>
    /// <returns></returns>
    ClientApp GetClient(string appId);

    /// <summary>
    /// 删除前台应用配置
    /// </summary>
    /// <returns></returns>
    bool DeleteClient(string appId);
}
