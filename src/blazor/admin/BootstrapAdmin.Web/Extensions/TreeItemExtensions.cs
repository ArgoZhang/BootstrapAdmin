// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class TreeItemExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="navigations"></param>
    /// <param name="selectedItems"></param>
    /// <param name="render"></param>
    /// <param name="parentId"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static List<TreeViewItem<Navigation>> ToTreeItemList(this IEnumerable<Navigation> navigations, List<string> selectedItems, RenderFragment<Navigation> render, string? parentId = "0", TreeViewItem<Navigation>? parent = null)
    {
        var trees = new List<TreeViewItem<Navigation>>();
        var roots = navigations.Where(i => i.ParentId == parentId).OrderBy(i => i.Application).ThenBy(i => i.Order);
        foreach (var node in roots)
        {
            var item = new TreeViewItem<Navigation>(node)
            {
                Text = node.Name,
                Icon = node.Icon,
                IsActive = selectedItems.Any(v => node.Id == v),
                Parent = parent,
                Template = render,
                CheckedState = selectedItems.Any(i => i == node.Id) ? CheckboxState.Checked : CheckboxState.UnChecked
            };
            item.Items = ToTreeItemList(navigations, selectedItems, render, node.Id, item);
            trees.Add(item);
        }
        return trees;
    }
}
