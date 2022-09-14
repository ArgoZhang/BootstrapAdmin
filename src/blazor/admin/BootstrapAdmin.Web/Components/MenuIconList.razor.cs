// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.JSInterop;
using System.Data;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class MenuIconList
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// 获得/设置 IJSRuntime 实例
    /// </summary>
    [Inject]
    [NotNull]
    protected IJSRuntime? JSRuntime { get; set; }

    private string? IconDialogClassString => CssBuilder.Default("menu-icons")
        .AddClass("show", Open)
        .Build();

    private bool _init;

    /// <summary>
    /// OnAfterRenderAsync 方法
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!_init && Open)
        {
            _init = true;
            await JSRuntime.InvokeVoidAsync("$.bb_scrollspy");
        }
    }

    private async Task OnCloseIconDialog()
    {
        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
    }

    private async Task OnSelctedIcon()
    {
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }

        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
    }
}
