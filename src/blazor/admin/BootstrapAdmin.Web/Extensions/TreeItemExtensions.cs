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
    /// <returns></returns>
    public static List<TreeItem> ToTreeItemList(this IEnumerable<Navigation> navigations, List<string> selectedItems, RenderFragment<Navigation> render, string? parentId = "0")
    {
        var trees = new List<TreeItem>();
        var roots = navigations.Where(i => i.ParentId == parentId).OrderBy(i => i.Application).ThenBy(i => i.Order);
        foreach (var node in roots)
        {
            trees.Add(new TreeItem
            {
                Text = node.Name,
                Icon = node.Icon,
                Checked = selectedItems.Any(v => node.Id == v),
                Key = node.Id,
                Template = render(node),
                Items = ToTreeItemList(navigations, selectedItems, render, node.Id!)
            });
        }
        return trees;
    }
}
