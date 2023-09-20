// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Utils;

/// <summary>
/// 登录获取默认首页帮助类
/// </summary>
public static class LoginHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="returnUrl"></param>
    /// <param name="appId"></param>
    /// <param name="userService"></param>
    /// <param name="dictService"></param>
    /// <returns></returns>
    public static string GetDefaultUrl(BootstrapAppContext context, string? returnUrl, string? appId, IUser userService, IDict dictService)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            // 查找 User 设置的默认应用
            var userName = context.UserName;
            var defaultAppId = context.AppId;
            var schema = context.BaseUri.Scheme;
            var host = context.BaseUri.Host;
            appId ??= userService.GetAppIdByUserName(userName) ?? defaultAppId;

            if (appId == defaultAppId && dictService.GetEnableDefaultApp())
            {
                // 开启默认应用
                appId = dictService.GetApps().FirstOrDefault(d => d.Key != defaultAppId).Key;
            }

            if (!string.IsNullOrEmpty(appId))
            {
                var appUrl = dictService.GetHomeUrlByAppId(appId);
                if (!string.IsNullOrEmpty(appUrl))
                {
                    returnUrl = string.Format(appUrl, schema, host).TrimEnd('/');
                }
            }
        }

        return returnUrl ?? "/Admin/Index";
    }

    /// <summary>
    /// 将字典表中的配置 1-Login-Gitee 转化为 gitee
    /// </summary>
    /// <param name="loginTheme"></param>
    /// <returns></returns>
    public static string? GetCurrentLoginTheme(string loginTheme)
    {
        string? ret = null;
        var segs = loginTheme.Split('-');
        if (segs.Length == 3)
        {
            ret = segs[2].ToLowerInvariant();
        }
        return ret;
    }
}
