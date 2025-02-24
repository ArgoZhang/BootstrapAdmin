﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.JSInterop;

namespace BootstrapAdmin.Web.Components.Components;

/// <summary>
/// FAIconList 组件
/// </summary>
[JSModuleAutoLoader("./Components/Components/FAIconList.razor.js", JSObjectReference = true)]
public partial class FAIconList
{
    private string? ClassString => CssBuilder.Default("icon-list")
        .AddClass("is-catalog", ShowCatalog)
        .AddClass("is-dialog", ShowCopyDialog)
        .Build();

    /// <summary>
    /// 获得/设置 点击时是否显示高级拷贝弹窗 默认 false 直接拷贝到粘贴板
    /// </summary>
    [Parameter]
    public bool ShowCopyDialog { get; set; }

    /// <summary>
    /// 获得/设置 是否显示目录 默认 false
    /// </summary>
    [Parameter]
    public bool ShowCatalog { get; set; }

    /// <summary>
    /// 获得/设置 高级弹窗 Header 显示文字
    /// </summary>
    [Parameter]
    [NotNull]
    public string? DialogHeaderText { get; set; }

    /// <summary>
    /// 获得/设置 当前选择图标
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// 获得/设置 当前选择图标回调方法
    /// </summary>
    [Parameter]
    public EventCallback<string?> IconChanged { get; set; }

    /// <summary>
    /// 获得/设置 拷贝成功提示文字
    /// </summary>
    [Parameter]
    public string? CopiedTooltipText { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        DialogHeaderText ??= "请选择图标";
        CopiedTooltipText ??= "已拷贝";
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override Task InvokeInitAsync() => InvokeVoidAsync("init", Id, Interop, nameof(UpdateIcon), nameof(ShowDialog));

    /// <summary>
    /// UpdateIcon 方法由 JS Invoke 调用
    /// </summary>
    /// <param name="icon"></param>
    [JSInvokable]
    public async Task UpdateIcon(string icon)
    {
        Icon = icon;
        if (IconChanged.HasDelegate)
        {
            await IconChanged.InvokeAsync(Icon);
        }
        else
        {
            StateHasChanged();
        }
    }

    /// <summary>
    /// ShowDialog 方法由 JS Invoke 调用
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public Task ShowDialog(string text) => DialogService.ShowCloseDialog<IconDialog>(DialogHeaderText, parameters =>
    {
        parameters.Add(nameof(IconDialog.IconName), text);
    });
}
