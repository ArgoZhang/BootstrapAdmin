// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Shared;
using BootstrapAdmin.Web.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;

namespace BootstrapAdmin.Web.Pages.Account;

/// <summary>
/// 
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

    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery]
    [Parameter]
    public string? AppId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery]
    [Parameter]
    public string? View { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    [Inject]
    [NotNull]
    private IOptionsMonitor<BootstrapAdminAuthenticationOptions>? Options { get; set; }

    /// <summary>
    /// BuildRenderTree 方法
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Options.CurrentValue.Domain)
        {
            builder.OpenComponent<Redirect>(0);
            builder.AddAttribute(1, nameof(Redirect.Url), "/Admin/Index");
            builder.CloseComponent();
        }
        else
        {
            if (!string.IsNullOrEmpty(View))
            {
                View = $"0-{View}";
            }
            var view = LoginHelper.GetCurrentLoginTheme(View ?? DictsService.GetCurrentLogin());
            var componentType = view switch
            {
                "gitee" => typeof(AdminLoginGitee),
                _ => typeof(AdminLogin)
            };
            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, nameof(AdminLogin.ReturnUrl), ReturnUrl);
            builder.AddAttribute(2, nameof(AdminLogin.AppId), AppId);
            builder.CloseComponent();

            builder.OpenComponent<AdminLoginFooter>(3);
            builder.CloseComponent();
        }
    }
}
