// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

namespace BootstrapAdmin.Web.Components.Components;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TItem"></typeparam>
public abstract class AssignmentBase<TItem> : ComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<TItem>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<string>? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Action<List<string>>? OnValueChanged { get; set; }
}
