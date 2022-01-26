// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class AdminAlert
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Color Color { get; set; } = Color.Danger;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool IsShow { get; set; } = true;
}
