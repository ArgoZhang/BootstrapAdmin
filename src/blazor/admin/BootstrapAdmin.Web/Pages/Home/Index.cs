// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace BootstrapAdmin.Web.Pages.Home;

/// <summary>
/// 返回前台页面
/// </summary>
[Route("")]
[Route("Home")]
[Route("Index")]
[Route("Home/Index")]
[Authorize]
public class Index : ComponentBase
{
    [Inject]
    [NotNull]
    private NavigationManager? Navigation { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UsersService { get; set; }

    [NotNull]
    private string? Url { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        // 查看是否自定义前台
        Url = LoginHelper.GetDefaultUrl(Context, null, null, UsersService, DictsService);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="firstRender"></param>
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            var routes = GetType().GetCustomAttributes<RouteAttribute>();
            if (routes.Any(i => $"{Navigation.BaseUri}{i.Template}".TrimEnd('/').Equals(Url, StringComparison.OrdinalIgnoreCase)))
            {
                Url = "Admin/Index";
            }
        }
        Navigation.NavigateTo(Url, true);
    }
}
