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
                Component = BootstrapDynamicComponent.CreateComponent<RoleUser>(new Dictionary<string, object>
                {
                    [nameof(RoleUser.RoleId)] = role.Id!
                })
            };

            await DialogService.Show(option);
        }

        private async Task OnAssignmentGroups(Role role)
        {
            var option = new DialogOption()
            {
                Title = $"分配部门 - {role}",
                Component = BootstrapDynamicComponent.CreateComponent<RoleGroup>(new Dictionary<string, object>
                {
                    [nameof(RoleGroup.RoleId)] = role.Id!
                })
            };

            await DialogService.Show(option);
        }

        private async Task OnAssignmentMenus(Role role)
        {
            var option = new DialogOption()
            {
                Title = $"分配菜单 - {role}",
                Component = BootstrapDynamicComponent.CreateComponent<RoleMenu>(new Dictionary<string, object>
                {
                    [nameof(RoleMenu.RoleId)] = role.Id!
                })
            };

            await DialogService.Show(option);
        }

        private async Task OnAssignmentApps(Role role)
        {
            var option = new DialogOption()
            {
                Title = $"分配应用 - {role}",
                Component = BootstrapDynamicComponent.CreateComponent<RoleApp>(new Dictionary<string, object>
                {
                    [nameof(RoleApp.RoleId)] = role.Id!
                })
            };

            await DialogService.Show(option);
        }

    }
}
