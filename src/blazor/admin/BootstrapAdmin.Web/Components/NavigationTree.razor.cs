// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

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

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        InternalItems = Items.ToTreeItemList(Value, RenderTreeItem);
    }

    private Task OnTreeItemChecked(List<TreeItem> items)
    {
        Value = items.Select(i => i.Key!.ToString()!).ToList();
        OnValueChanged(Value);
        return Task.CompletedTask;
    }

    private string GetApp(string? app) => DictService.GetApps().FirstOrDefault(i => i.Key == app).Value ?? "未设置";
}
