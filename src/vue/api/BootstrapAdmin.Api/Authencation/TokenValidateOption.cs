// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Api.Authencation;

/// <summary>
/// 
/// </summary>
public class TokenValidateOption
{
    /// <summary>
    /// 
    /// </summary>
    public string Issuer { get; set; } = "BA";

    /// <summary>
    /// 
    /// </summary>
    public string Audience { get; set; } = "api";

    /// <summary>
    /// 
    /// </summary>
    public int Expires { get; set; } = 5;

    /// <summary>
    /// 
    /// </summary>
    public string SecurityKey { get; set; } = "BootstrapAdmin-V1.1";

}
