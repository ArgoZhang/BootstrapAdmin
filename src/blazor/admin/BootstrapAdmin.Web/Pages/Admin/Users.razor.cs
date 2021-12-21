using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Users
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
    private IRole? RoleService { get; set; }

    private async Task OnAssignmentGroups(User user)
    {
        var groups = GroupService.GetAll().ToSelectedItemList();
        var values = GroupService.GetGroupsByUserId(user.Id);

        await DialogService.ShowAssignmentDialog($"分配部门 - 当前用户: {user}", groups, values, () =>
        {
            var ret = GroupService.SaveGroupsByUserId(user.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentRoles(User user)
    {
        var groups = RoleService.GetAll().ToSelectedItemList();
        var values = RoleService.GetRolesByUserId(user.Id);

        await DialogService.ShowAssignmentDialog($"分配角色 - 当前用户: {user}", groups, values, () =>
        {
            var ret = RoleService.SaveRolesByUserId(user.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }
}
