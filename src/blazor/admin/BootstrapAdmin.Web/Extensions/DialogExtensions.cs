// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Components;

namespace BootstrapAdmin.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class DialogExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static Task ShowAssignmentDialog(this DialogService dialogService, string title, List<SelectedItem> items, List<string> value, Func<Task<bool>> saveCallback, ToastService? toast) => dialogService.ShowDialog<Assignment, SelectedItem>(title, items, value, saveCallback, toast);

    /// <summary>
    /// 
    /// </summary>
    public static Task ShowNavigationDialog(this DialogService dialogService, string title, List<Navigation> items, List<string> value, Func<Task<bool>> saveCallback, ToastService? toast) => dialogService.ShowDialog<NavigationTree, Navigation>(title, items, value, saveCallback, toast);

    private static Task ShowDialog<TBody, TItem>(this DialogService dialogService, string title, List<TItem> items, List<string> value, Func<Task<bool>> saveCallback, ToastService? toast) where TBody : AssignmentBase<TItem> => dialogService.ShowSaveDialog<TBody>(title, async () =>
    {
        var ret = await saveCallback();
        if (toast != null)
        {
            if (ret)
            {
                await toast.Success("分配操作", "操作成功！");
            }
            else
            {
                await toast.Error("分配操作", "操作失败，请联系相关管理员！");
            }
        }
        return ret;
    },
    new Dictionary<string, object?>
    {
        [nameof(AssignmentBase<TItem>.Items)] = items,
        [nameof(AssignmentBase<TItem>.Value)] = value,
        [nameof(AssignmentBase<TItem>.OnValueChanged)] = new Action<List<string>>(v =>
        {
            value.Clear();
            value.AddRange(v);
        })
    },
    op =>
    {
        op.IsScrolling = true;
    });
}
