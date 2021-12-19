using BootstrapAdmin.Web.Components;

namespace BootstrapAdmin.Web.Pages.Admin
{
    public partial class Menus
    {
        [Inject]
        [NotNull]
        private DialogService? DialogService { get; set; }

        private async Task OnAssignmentRoles(DataAccess.Models.Navigation menu)
        {
            var option = new DialogOption()
            {
                Title = $"分配角色 - {menu}",
                Component = BootstrapDynamicComponent.CreateComponent<MenuRole>(new Dictionary<string, object>
                {
                    [nameof(MenuRole.MenuId)] = menu.Id!
                })
            };

            await DialogService.Show(option);
        }
    }
}
