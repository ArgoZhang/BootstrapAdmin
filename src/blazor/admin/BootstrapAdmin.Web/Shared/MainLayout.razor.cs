using Bootstrap.Security.Blazor;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BootstrapAdmin.Web.Shared
{
    /// <summary>
    /// MainLayout 布局类
    /// </summary>
    public partial class MainLayout
    {
        private IEnumerable<MenuItem>? MenuItems { get; set; }

        [Inject]
        [NotNull]
        private INavigation? NavigationsService { get; set; }

        [Inject]
        [NotNull]
        private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        [Inject]
        [NotNull]
        private IDict? DictsService { get; set; }

        [Inject]
        [NotNull]
        private IUser? UsersService { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? Context { get; set; }

        [Inject]
        [NotNull]
        private IBootstrapAdminService? SecurityService { get; set; }

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        private string? Title { get; set; }

        private string? Footer { get; set; }

        private string? DisplayName { get; set; }

        private string? UserName { get; set; }

        private bool Lock { get; set; }

        private int LockInterval { get; set; }

        [NotNull]
        private string? Icon { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            UserName = state.User.Identity?.Name;

            if (!string.IsNullOrEmpty(UserName))
            {
                var user = UsersService.GetUserByUserName(UserName);
                DisplayName = user?.DisplayName ?? "未注册账户";
                Context.UserName = UserName;
                Context.DisplayName = DisplayName;
                Icon = string.IsNullOrEmpty(user?.Icon) ? "/images/uploader/default.jpg" : GetIcon(user.Icon);

                MenuItems = NavigationsService.GetAllMenus(UserName).ToAdminMenus();
            }

            Title = DictsService.GetWebTitle();
            Footer = DictsService.GetWebFooter();

            string GetIcon(string icon) => icon.Contains("://", StringComparison.OrdinalIgnoreCase) ? icon : string.Format("{0}{1}", DictsService.GetIconFolderPath(), icon);
            Lock = DictsService.GetAutoLockScreen();
            LockInterval = Convert.ToInt32(DictsService.GetAutoLockScreenInterval());
        }

        private Task<bool> OnAuthorizing(string url) => SecurityService.AuhorizingNavigation(Context.UserName, url);

        private async Task OnErrorHandleAsync(ILogger logger, Exception ex)
        {
            await ToastService.Error(Title, ex.Message);

            logger.LogError(ex, "ErrorLogger");
        }
    }
}
