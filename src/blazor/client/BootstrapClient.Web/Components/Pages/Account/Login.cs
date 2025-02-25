// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapClient.Web.Components.Shared;
using BootstrapClient.Web.Core;
using BootstrapClient.Web.Extensions;
using BootstrapClient.Web.Services;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Options;

namespace BootstrapClient.Web.Components.Pages.Account;

/// <summary>
/// Login 组件
/// </summary>
[Layout(typeof(LoginLayout))]
[Route("/Account/Login")]
public class Login : ComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery]
    [Parameter]
    public string? ReturnUrl { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="firstRender"></param>
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        var adminUrl = DictsService.GetAdminUrl();
        if (!string.IsNullOrEmpty(adminUrl))
        {
            Context.BaseUri = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);
            Context.AdminUrl = string.Format(adminUrl, Context.BaseUri.Scheme, Context.BaseUri.Host).TrimEnd('/');
        }
        var url = Context.CombinePath($"/Account/Login?ReturnUrl={ReturnUrl}&AppId={Context.AppId}");
        NavigationManager.NavigateTo(url, true);
    }
}
