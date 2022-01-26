// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Groups
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    [Inject]
    [NotNull]
    private IRole? RoleService { get; set; }

    private async Task OnAssignmentUsers(Group group)
    {
        var users = UserService.GetAll().ToSelectedItemList();
        var values = UserService.GetUsersByGroupId(group.Id);

        await DialogService.ShowAssignmentDialog($"分配用户 - 当前部门: {group}", users, values, () =>
        {
            var ret = UserService.SaveUsersByGroupId(group.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentRoles(Group group)
    {
        var users = RoleService.GetAll().ToSelectedItemList();
        var values = RoleService.GetRolesByGroupId(group.Id);

        await DialogService.ShowAssignmentDialog($"分配角色 - 当前部门: {group}", users, values, () =>
        {
            var ret = RoleService.SaveRolesByGroupId(group.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }
}
