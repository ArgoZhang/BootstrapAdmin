// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Components.Layout;

namespace BootstrapAdmin.Web.Components.Pages.Account;

/// <summary>
/// Login 组件
/// </summary>
[Layout(typeof(LoginLayout))]
[Route("/Account/Logout")]
public class Logout : ComponentBase
{
    [Inject, NotNull]
    private NavigationManager? NavigationManager { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="firstRender"></param>
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        NavigationManager.NavigateTo("Login/Logout", true);
    }
}
