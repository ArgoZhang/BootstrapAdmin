// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ParentMenuTree
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public string? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [NotNull]
    private List<TreeViewItem<Navigation>>? Items { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var items = NavigationService.GetAllMenus(Context.UserName);
        Items = items.ToTreeItemList(new List<string> { Value }, RenderTreeItem);
    }

    private async Task OnTreeItemClick(TreeViewItem<Navigation> item)
    {
        Value = item.Value.Id;
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }

    private string GetApp(string? app) => DictService.GetApps().FirstOrDefault(i => i.Key == app).Value ?? "未设置";
}
