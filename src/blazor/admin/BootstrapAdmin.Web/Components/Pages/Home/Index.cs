// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.Web.Components.Layout;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace BootstrapAdmin.Web.Pages.Home;

/// <summary>
/// 返回前台页面
/// </summary>
[Route("/")]
[Route("/Home")]
[Route("/Home/Index")]
[Authorize]
[Layout(typeof(LoginLayout))]
public class Index : ComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery]
    [Parameter]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery]
    [Parameter]
    public string? AppId { get; set; }

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

    [Inject]
    [NotNull]
    private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    private bool _render = true;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        _render = false;
        await base.OnParametersSetAsync();

        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        Context.UserName = state.User.Identity?.Name;
        Context.BaseUri = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);
        _render = true;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override bool ShouldRender() => _render;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="firstRender"></param>
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            var url = LoginHelper.GetDefaultUrl(Context, ReturnUrl, AppId, UsersService, DictsService);
            Navigation.NavigateTo(url);
        }
    }
}
