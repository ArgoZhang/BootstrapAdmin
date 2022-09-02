// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapClient.Web.Core;

/// <summary>
/// Dict 字典表接口
/// </summary>
public interface IDict
{
    /// <summary>
    /// 获取当前系统配置是否为演示模式
    /// </summary>
    /// <returns></returns>
    bool IsDemo();

    /// <summary>
    /// 获取 站点 Title 配置信息
    /// </summary>
    /// <returns></returns>
    string GetWebTitle(string appId);

    /// <summary>
    /// 获取站点 Footer 配置信息
    /// </summary>
    /// <returns></returns>
    string GetWebFooter(string appId);

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
    /// <returns></returns>
    string GetAdminUrl();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetNotificationUrl(string appId);
}
