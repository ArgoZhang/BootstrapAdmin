// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class WebHostExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="webHost"></param>
    /// <param name="logoFolder"></param>
    /// <param name="logoFile"></param>
    /// <returns></returns>
    public static string CombineLogoFile(this IWebHostEnvironment webHost, string logoFolder, string logoFile) => Path.Combine(webHost.WebRootPath, logoFolder.Replace("/", "\\").TrimStart('\\'), logoFile);

}
