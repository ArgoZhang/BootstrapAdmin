// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using Microsoft.JSInterop;

namespace BootstrapAdmin.Web.Components.Components;

/// <summary>
/// 
/// </summary>
public partial class AdminLogin : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    protected string? Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected bool AllowMobile { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    protected bool UseMobileLogin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected bool AllowOAuth { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    protected bool RememberPassword { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected ElementReference LoginForm { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected string? PostUrl { get; set; }

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

    /// <summary>
    /// 
    /// </summary>
    [Inject]
    [NotNull]
    protected IDict? DictsService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Inject]
    [NotNull]
    protected ILogin? LoginService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Inject]
    [NotNull]
    protected WebClientService? WebClientService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected string? ClassString => CssBuilder.Default("login-wrap")
        .AddClass("is-mobile", UseMobileLogin)
        .Build();

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = DictsService.GetWebTitle();
        PostUrl = QueryHelper.AddQueryString("Account/Login", new Dictionary<string, string?>
        {
            ["ReturnUrl"] = ReturnUrl,
            ["AppId"] = AppId
        });
    }

    /// <summary>
    /// 
    /// </summary>
    protected void OnClickSwitchButton()
    {
        var rem = RememberPassword ? "true" : "false";
        PostUrl = QueryHelper.AddQueryString(UseMobileLogin ? "Account/Mobile" : "Account/Login", new Dictionary<string, string?>()
        {
            [nameof(ReturnUrl)] = ReturnUrl,
            ["AppId"] = AppId,
            ["remember"] = rem
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="remember"></param>
    /// <returns></returns>
    protected Task OnRememberPassword(bool remember)
    {
        OnClickSwitchButton();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    protected void OnSignUp()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    protected void OnForgotPassword()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="result"></param>
    [JSInvokable]
    public async Task Log(string userName, bool result)
    {
        var clientInfo = await WebClientService.GetClientInfo();
        LoginService.Log(userName, clientInfo.Ip, clientInfo.OS, clientInfo.Browser, clientInfo.City, clientInfo.UserAgent, result);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Interop != null)
            {
                Interop.Dispose();
                Interop = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
