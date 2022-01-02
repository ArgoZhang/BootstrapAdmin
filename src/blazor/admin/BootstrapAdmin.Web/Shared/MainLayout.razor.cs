using Bootstrap.Security.Blazor;
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
        private NavigationManager? NavigationManager { get; set; }

        [Inject]
        [NotNull]
        private INavigation? NavigationsService { get; set; }

        [Inject]
        [NotNull]
        private IDict? DictsService { get; set; }

        [Inject]
        [NotNull]
        private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

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
                DisplayName = UsersService.GetDisplayName(UserName);
                Context.UserName = UserName;
                Context.DisplayName = DisplayName;

                MenuItems = NavigationsService.GetAllMenus(UserName).ToAdminMenus();
            }

            Title = DictsService.GetWebTitle();
            Footer = DictsService.GetWebFooter();
        }

        private Task<bool> OnAuthorizing(string url) => SecurityService.AuhorizingNavigation(Context.UserName, url);

        private void OnLogout() => NavigationManager.NavigateTo("/Account/Logout", true);

        private async Task OnErrorHandleAsync(ILogger logger, Exception ex)
        {
            await ToastService.Error(Title, ex.Message);

            logger.LogError(ex, "ErrorLogger");
        }
    }
}
