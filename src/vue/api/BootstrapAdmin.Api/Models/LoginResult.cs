// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Api.Models;

/// <summary>
/// 
/// </summary>
public class LoginResult
{
    /// <summary>
    /// 
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RefershToken { get; set; }
}
