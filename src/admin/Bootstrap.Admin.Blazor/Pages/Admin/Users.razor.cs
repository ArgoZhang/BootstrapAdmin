using BootstrapBlazor.Components;
using Bootstrap.DataAccess;
using Task = System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Admin.Blazor.Pages.Admin
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

        private Task<IEnumerable<User>> OnQueryAsync()
        {
            return Task.FromResult(UserHelper.Retrieves());
        }

        private async Task OnAssignmentDept(IEnumerable<User> users)
        {
            if (users.Count() != 0)
            {
                var option = new DialogOption()
                {
                    Title = "部门授权",
                    BodyTemplate = BootstrapDynamicComponent.CreateComponent<CheckboxList<string>>(new Dictionary<string, object>
                    {
                        [nameof(CheckboxList<string>.Items)] = GroupHelper.Retrieves().Select(s => new SelectedItem(s.GroupCode, s.GroupName))
                    }).Render()
                };

                await DialogService.Show(option);
            }
            else
            {
                await ToastService.Warning("部门授权", "请选择一个用户");
            }
        }

        private async Task OnAssignmentRoles(IEnumerable<User> users)
        {
            if (users.Count() != 0)
            {
                var option = new DialogOption()
                {
                    Title = "分配角色",
                    BodyTemplate = BootstrapDynamicComponent.CreateComponent<CheckboxList<string>>(new Dictionary<string, object>
                    {
                        [nameof(CheckboxList<string>.Items)] = RoleHelper.Retrieves().Select(s => new SelectedItem(s.Id!, s.RoleName) { Active = s.Checked == "" ? false : true })
                    }).Render()
                };

                await DialogService.Show(option);
            }
            else
            {
                await ToastService.Warning("分配角色", "请选择一个用户");
            }
        }
    }
}
