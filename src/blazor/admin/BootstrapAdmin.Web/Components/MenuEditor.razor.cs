// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Components;

public partial class MenuEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Navigation? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? ParementMenus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? Targets { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? Apps { get; set; }
}
