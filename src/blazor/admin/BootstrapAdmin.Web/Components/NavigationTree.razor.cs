// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class NavigationTree
{
    [NotNull]
    private List<TreeItem>? InternalItems { get; set; }

    [NotNull]
    private Tree? MenusTree { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<Navigation>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<string>? Value { get; set; }

    /// <summary>
    /// 关闭弹窗回调委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Func<Task>? OnClose { get; set; }

    /// <summary>
    /// 保存按钮回调委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Func<List<string?>, Task>? OnSave { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        InternalItems = Items.ToTreeItemList(Value, RenderTreeItem);
    }

    private string GetApp(string? app) => DictService.GetApps().FirstOrDefault(i => i.Key == app).Value ?? "未设置";

    private Task OnClickClose() => OnClose();

    private Task OnClickSave() => OnSave(MenusTree.GetCheckItems().Select(i => i.Key?.ToString()).ToList());
}
