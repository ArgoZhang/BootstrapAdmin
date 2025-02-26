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
[Layout(typeof(LoginLayout))]
[Authorize]
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

    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        if (HttpContext != null)
        {
            var userName = HttpContext?.User.Identity?.Name;
            Context.UserName = userName;
        }
        Context.BaseUri ??= NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrEmpty(Context.UserName))
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            Context.UserName = state.User.Identity?.Name;
        }
        var url = LoginHelper.GetDefaultUrl(Context, ReturnUrl, AppId, UsersService, DictsService);
        NavigationManager.NavigateTo(url);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override bool ShouldRender() => false;
}
