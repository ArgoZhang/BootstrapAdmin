using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Components;

namespace BootstrapAdmin.Web.Pages.Admin
{
    public partial class Roles
    {
        [Inject]
        [NotNull]
        private DialogService? DialogService { get; set; }

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
            var option = new DialogOption()
            {
                Title = $"分配部门 - {role}",
            };

            await DialogService.Show(option);
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
