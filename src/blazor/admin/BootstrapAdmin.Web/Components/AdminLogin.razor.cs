﻿using BootstrapAdmin.Web.Core;
using Microsoft.JSInterop;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class AdminLogin
{
    private string? Title { get; set; }

    private bool AllowMobile { get; set; } = true;

    private bool UseMobileLogin { get; set; }

    private bool AllowOAuth { get; set; } = true;

    private bool RememberPassword { get; set; }

    private ElementReference LoginForm { get; set; }

    private string? PostUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? AppId { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    [Inject]
    [NotNull]
    private IJSRuntime? JSRuntime { get; set; }

    private string? ClassString => CssBuilder.Default("login-wrap")
        .AddClass("is-mobile", UseMobileLogin)
        .Build();

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = DictsService.GetWebTitle();

        PostUrl = QueryHelper.AddQueryString("/Account/Login", new Dictionary<string, string?>
        {
            ["ReturnUrl"] = ReturnUrl,
            ["AppId"] = AppId
        });
    }

    void OnClickSwitchButton()
    {
        var rem = RememberPassword ? "true" : "false";
        PostUrl = QueryHelper.AddQueryString(UseMobileLogin ? "/Account/Mobile" : "/Account/Login", new Dictionary<string, string?>()
        {
            [nameof(ReturnUrl)] = ReturnUrl,
            ["AppId"] = AppId,
            ["remember"] = rem
        });
    }

    /// <summary>
    /// OnAfterRenderAsync 方法
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            // register javascript
            await JSRuntime.InvokeVoidAsync("$.login", LoginForm, "api/Login");
        }
    }

    Task OnRememberPassword(bool remember)
    {
        OnClickSwitchButton();
        return Task.CompletedTask;
    }

    void OnSignUp()
    {

    }

    void OnForgotPassword()
    {

    }
}
