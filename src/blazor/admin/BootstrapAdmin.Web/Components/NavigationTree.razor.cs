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
    private List<TreeViewItem<Navigation>>? Items { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<Navigation>? AllMenus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<string>? SelectedMenus { get; set; }

    /// <summary>
    /// 保存按钮回调委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Func<List<string>, Task<bool>>? OnSave { get; set; }

    [CascadingParameter]
    private Func<Task>? CloseDialogAsync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Items = AllMenus.ToTreeItemList(SelectedMenus, RenderTreeItem);
    }

    private string GetApp(string? app) => DictService.GetApps().FirstOrDefault(i => i.Key == app).Value ?? "未设置";

    private async Task OnClickClose()
    {
        if (CloseDialogAsync != null)
        {
            await CloseDialogAsync();
        }
    }

    private Task OnTreeItemChecked(List<TreeViewItem<Navigation>> items)
    {
        SelectedMenus = items.SelectMany(i =>
        {
            var ret = new List<string>
            {
                i.Value.Id
            };
            if (i.Parent != null)
            {
                ret.Add(i.Parent.Value.Id);
            }
            return ret;
        }).Distinct().ToList();
        return Task.CompletedTask;
    }

    private async Task OnClickSave()
    {
        var ret = await OnSave(SelectedMenus);
        if (ret)
        {
            await OnClickClose();
            await ToastService.Success("分配菜单操作", "操作成功！");

        }
        else
        {
            await ToastService.Error("分配菜单操作", "操作失败，请联系相关管理员！");
        }
    }
}
