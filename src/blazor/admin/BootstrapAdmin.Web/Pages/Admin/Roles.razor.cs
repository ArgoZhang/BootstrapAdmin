using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;

namespace BootstrapAdmin.Web.Pages.Admin
{
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

        private async Task OnAssignmentUsers(Role role)
        {
            var option = new DialogOption()
            {
                Title = $"分配用户 - {role}",
            };

            await DialogService.Show(option);
        }

        private async Task OnAssignmentGroups(Role role)
        {
            var groups = GroupService.GetAll().ToSelectedItemList();
            var values = GroupService.GetGroupsByRoleId(role.Id);

            await DialogService.ShowAssignmentDialog($"分配部门 - {role.RoleName}", groups, values, () =>
            {
                var ret = GroupService.SaveGroupsByRoleId(role.Id, values);
                return Task.FromResult(ret);
            }, ToastService);
        }

        private async Task OnAssignmentMenus(Role role)
        {
            var option = new DialogOption()
            {
                Title = $"分配菜单 - {role}",
            };

            await DialogService.Show(option);
        }

        private async Task OnAssignmentApps(Role role)
        {
            var option = new DialogOption()
            {
                Title = $"分配应用 - {role}",
            };

            await DialogService.Show(option);
        }

    }
}
