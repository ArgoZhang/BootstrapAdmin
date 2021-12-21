using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;

namespace BootstrapAdmin.Web.Pages.Admin;

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
}
