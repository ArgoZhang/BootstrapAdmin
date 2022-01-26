// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class UserLogin
{
    private string? UserName { get; set; }

    private string? Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

#if DEBUG
        UserName = "Admin";
        Password = "123789";
#endif
    }
}
