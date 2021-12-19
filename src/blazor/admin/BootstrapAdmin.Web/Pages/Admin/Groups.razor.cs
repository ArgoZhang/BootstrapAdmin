using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Components;

namespace BootstrapAdmin.Web.Pages.Admin
{
    public partial class Groups
    {
        [Inject]
        [NotNull]
        private DialogService? DialogService { get; set; }

        private async Task OnAssignmentUsers(Group group)
        {
            var option = new DialogOption()
            {
                Title = $"分配用户 - {group}",
                Component = BootstrapDynamicComponent.CreateComponent<GroupUser>(new Dictionary<string, object>
                {
                    [nameof(GroupUser.GroupId)] = group.Id!
                }),
                ShowFooter = false
            };

            await DialogService.Show(option);
        }

        private async Task OnAssignmentRoles(Group group)
        {
            var option = new DialogOption()
            {
                Title = $"分配角色 - {group}"
            };
            var items = new List<SelectedItem>() { new SelectedItem("1", "角色1"), new SelectedItem("2", "角色2") };
            option.Component = BootstrapDynamicComponent.CreateComponent<CheckboxList<IEnumerable<string>>>(new Dictionary<string, object>
            {
                [nameof(CheckboxList<IEnumerable<string>>.Value)] = new List<string>() { "1" },
                [nameof(CheckboxList<IEnumerable<string>>.Items)] = items
            });
            option.FooterTemplate = builder =>
            {
                builder.OpenComponent<Button>(0);
                builder.AddAttribute(1, nameof(Button.Color), Color.Primary);
                builder.AddAttribute(2, nameof(Button.Text), "保存");
                builder.AddAttribute(3, nameof(Button.Icon), "fa fa-save");
                builder.AddAttribute(4, nameof(Button.OnClickWithoutRender), async () =>
                {
                    var t = items;
                    await option.Dialog.Close();
                });
                builder.CloseComponent();
            };
            await DialogService.Show(option);
        }
    }
}
