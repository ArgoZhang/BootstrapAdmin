// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapClient.Web.Services;

namespace BootstrapClient.Web.Extensions;

/// <summary>
/// BootstrapAppContext 扩展方法类
/// </summary>
public static class BootstrapAppContextExtensions
{
    /// <summary>
    /// 合并路径
    /// </summary>
    /// <param name="context"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string CombinePath(this BootstrapAppContext context, string? url)
    {
        url ??= "";
        if (!string.IsNullOrEmpty(context.AdminUrl))
        {
            url = string.Join('/', context.AdminUrl, url.TrimStart('/'));
        }
        else
        {
            url = "#";
        }
        return url;
    }
}
