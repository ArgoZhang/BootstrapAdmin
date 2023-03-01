// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Roles
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IGroup? GroupService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    [Inject]
    [NotNull]
    private IApp? AppService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    private async Task OnAssignmentUsers(Role role)
    {
        var users = UserService.GetAll().ToSelectedItemList();
        var values = UserService.GetUsersByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"分配用户 - 当前角色: {role.RoleName}", users, values, () =>
        {
            var ret = UserService.SaveUsersByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentGroups(Role role)
    {
        var groups = GroupService.GetAll().ToSelectedItemList();
        var values = GroupService.GetGroupsByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"分配部门 - 当前角色: {role.RoleName}", groups, values, () =>
        {
            var ret = GroupService.SaveGroupsByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentMenus(Role role)
    {
        var menus = NavigationService.GetAllMenus(AppContext.UserName);
        var values = NavigationService.GetMenusByRoleId(role.Id);

        await DialogService.ShowNavigationDialog($"分配菜单 - 当前角色: {role.RoleName}", menus, values, items =>
        {
            var ret = NavigationService.SaveMenusByRoleId(role.Id, items);
            return Task.FromResult(ret);
        });
    }

    private async Task OnAssignmentApps(Role role)
    {
        var apps = DictService.GetApps();
        var values = AppService.GetAppsByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"分配应用 - 当前角色: {role.RoleName}", apps.ToSelectedItemList(), values, () =>
        {
            var ret = AppService.SaveAppsByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }
}
