using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Components;

namespace BootstrapAdmin.Web.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Users
    {
        [Inject]
        [NotNull]
        private DialogService? DialogService { get; set; }

        private async Task OnAssignmentGroups(User user)
        {
            var option = new DialogOption()
            {
                Title = $"分配部门 - {user}",
                Component = BootstrapDynamicComponent.CreateComponent<UserGroup>(new Dictionary<string, object>
                {
                    [nameof(UserGroup.UserId)] = user.Id!
                })
            };

            await DialogService.Show(option);
        }

        private async Task OnAssignmentRoles(User user)
        {
            var option = new DialogOption()
            {
                Title = $"分配角色 - {user}",
                Component = BootstrapDynamicComponent.CreateComponent<UserRole>(new Dictionary<string, object>
                {
                    [nameof(UserGroup.UserId)] = user.Id!
                })
            };

            await DialogService.Show(option);
        }
    }
}
