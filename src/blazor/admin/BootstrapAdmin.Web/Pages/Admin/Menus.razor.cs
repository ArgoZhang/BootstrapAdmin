// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Security.Blazor;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Utils;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Menus
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IRole? RoleService { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private IBootstrapAdminService? AdminService { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    [NotNull]
    private List<SelectedItem>? Targets { get; set; }

    [NotNull]
    private List<SelectedItem>? Apps { get; set; }

    [NotNull]
    private List<SelectedItem>? ParamentMenus { get; set; }

    [CascadingParameter]
    private Func<Task>? ReloadMenu { get; set; }

    private ITableSearchModel? SearchModel { get; set; } = new MenusSearchModel();

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Targets = LookupHelper.GetTargets();
        Apps = DictService.GetApps().ToSelectedItemList();

        ParamentMenus = NavigationService.GetAllMenus(AppContext.UserName).Where(s => s.ParentId == "0").Select(s => new SelectedItem(s.Id, s.Name)).ToList();
        ParamentMenus.Insert(0, new SelectedItem("0", "请选择"));
    }

    private bool AuthorizeButton(string operate)
    {
        var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        return AdminService.AuthorizingBlock(AppContext.UserName, url, operate);
    }

    private async Task OnAssignmentRoles(DataAccess.Models.Navigation menu)
    {
        var roles = RoleService.GetAll().ToSelectedItemList();
        var values = RoleService.GetRolesByMenuId(menu.Id);

        await DialogService.ShowAssignmentDialog($"分配角色 - 当前菜单: {menu.Name}", roles, values, () =>
        {
            var ret = RoleService.SaveRolesByMenuId(menu.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private Task<QueryData<Navigation>> OnQueryAsync(QueryPageOptions options)
    {
        var navigations = NavigationService.GetAllMenus(AppContext.UserName);
        var menus = navigations.Where(m => m.ParentId == "0");

        // 处理模糊查询
        if (options.Searches.Any())
        {
            menus = menus.Where(options.Searches.GetFilterFunc<Navigation>(FilterLogic.Or));
        }

        //  处理 Filter 高级搜索
        if (options.CustomerSearches.Any() || options.Filters.Any())
        {
            menus = menus.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<Navigation>());
        }

        foreach (var item in menus)
        {
            item.HasChildren = navigations.Any(i => i.ParentId == item.Id);
        }

        return Task.FromResult(new QueryData<Navigation>()
        {
            IsFiltered = true,
            IsSearch = true,
            IsSorted = true,
            Items = menus
        });
    }

    private async Task OnAfterModifyAsync()
    {
        if (ReloadMenu != null)
        {
            await ReloadMenu();
        }
    }

    private Task<IEnumerable<TableTreeNode<Navigation>>> OnTreeExpand(Navigation menu)
    {
        var navigations = NavigationService.GetAllMenus(AppContext.UserName);
        return Task.FromResult(navigations.Where(m => m.ParentId == menu.Id).OrderBy(m => m.Order).AsEnumerable().Select(i => new TableTreeNode<Navigation>(i)));
    }

    private Task<IEnumerable<TableTreeNode<Navigation>>> TreeNodeConverter(IEnumerable<Navigation> items)
    {
        var ret = BuildTreeNodes(items, "0");
        return Task.FromResult(ret);

        IEnumerable<TableTreeNode<Navigation>> BuildTreeNodes(IEnumerable<Navigation> items, string parentId)
        {
            var navigations = NavigationService.GetAllMenus(AppContext.UserName);
            var ret = new List<TableTreeNode<Navigation>>();
            ret.AddRange(items.Where(i => i.ParentId == parentId).Select((nav, index) => new TableTreeNode<Navigation>(nav)
            {
                HasChildren = navigations.Any(i => i.ParentId == nav.Id),
                IsExpand = navigations.Any(i => i.ParentId == nav.Id),
                Items = BuildTreeNodes(navigations.Where(i => i.ParentId == nav.Id), nav.Id)
            }));
            return ret;
        }
    }

    private bool ModelEqualityComparer(Navigation x, Navigation y) => x.Id == y.Id;
}
