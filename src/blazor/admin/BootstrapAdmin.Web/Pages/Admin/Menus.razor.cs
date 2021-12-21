namespace BootstrapAdmin.Web.Pages.Admin;

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
        };

        await DialogService.Show(option);
    }
}
