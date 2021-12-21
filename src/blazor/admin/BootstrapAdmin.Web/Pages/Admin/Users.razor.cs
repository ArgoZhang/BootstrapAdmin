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

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        [Inject]
        [NotNull]
        private IGroup? GroupService { get; set; }
        private async Task OnAssignmentGroups(User user)
        {
            var groups = GroupService.GetAll().ToSelectedItemList();
            var values = GroupService.GetGroupsByUserId(user.Id);

            await DialogService.ShowAssignmentDialog($"分配部门 - {user}", groups, values, () =>
            {
                var ret = GroupService.SaveGroupsByUserId(user.Id, values);
                return Task.FromResult(ret);
            }, ToastService);
        }

        private async Task OnAssignmentRoles(User user)
        {
            var option = new DialogOption()
            {
                Title = $"分配角色 - {user}",
            };

            await DialogService.Show(option);
        }
    }
}
