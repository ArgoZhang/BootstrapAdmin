// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapAdmin.Web.Components.Components;

/// <summary>
/// AdminLoginGitee 组件
/// </summary>
[JSModuleAutoLoader("./Components/Components/AdminLoginGitee.razor.js", AutoInvokeInit = false, AutoInvokeDispose = false)]
public partial class AdminLoginGitee
{
    [Inject, NotNull]
    private IUser? UserService { get; set; }

    private LoginModel Model { get; set; } = new();

    private async Task OnLogin(EditContext context)
    {
        var auth = UserService.Authenticate(Model.UserName, Model.Password);
        if (auth)
        {
            await InvokeVoidAsync("login", $"Login/Login?ReturnUrl={ReturnUrl}&AppId={AppId}", Model.UserName, Model.RememberMe);
        }
    }
}
