// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.Web.Core;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BootstrapAdmin.Web.Components.Components;

[JSModuleAutoLoader("./Components/Components/AdminLoginGitee.razor.js", JSObjectReference = true)]
public partial class AdminLoginGitee
{
    private string? _userName;

    private string? _password;

    [Inject, NotNull]
    private ToastService? ToastService { get; set; }

    [Inject, NotNull]
    private IUser? UserService { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override Task InvokeInitAsync() => InvokeVoidAsync("init", Id, Interop);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public async Task<bool> TriggerLogin()
    {
        var ret = false;
        if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password))
        {
            ret = UserService.Authenticate(_userName, _password);
        }
        if (!ret)
        {
            await ToastService.Error("登录", "用户名或密码错误");
        }
        return ret;
    }
}
