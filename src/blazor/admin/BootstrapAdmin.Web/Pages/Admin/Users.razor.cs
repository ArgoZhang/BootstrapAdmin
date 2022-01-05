using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Validators;

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

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    private static bool GetDisabled(string? id) => !string.IsNullOrEmpty(id);

    private List<IValidator> UserRules { get; } = new List<IValidator>();

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        UserRules.Add(new UserNameValidator(UserService));
    }

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
