using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapAdmin.Web.Pages.Admin
{
    public partial class Settings
    {
        private bool IsDemo { get; set; }

        [NotNull]
        private AppInfo? AppInfo { get; set; }

        [NotNull]
        private List<SelectedItem>? Logins { get; set; }

        [NotNull]
        private List<SelectedItem>? Themes { get; set; }

        [Inject]
        [NotNull]
        private IDict? DictService { get; set; }

        [Inject]
        [NotNull]
        private IUser? UserService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            IsDemo = DictService.IsDemo();
            Logins = DictService.GetLogins().ToSelectedItemList();
            Themes = DictService.GetThemes().ToSelectedItemList();
            AppInfo = new();
        }

        private Task OnSaveTitle(EditContext context)
        {
            return Task.CompletedTask;
        }

        private Task OnSaveFooter(EditContext context)
        {
            return Task.CompletedTask;
        }

        private Task OnSaveLogin(EditContext context)
        {
            return Task.CompletedTask;
        }

        private Task OnSaveAuthUrl(EditContext context)
        {
            return Task.CompletedTask;
        }

        private Task OnSaveTheme(EditContext context)
        {
            return Task.CompletedTask;
        }

        private void OnSaveDemo()
        {
            IsDemo = AppInfo.IsDemo;
        }
    }
}
