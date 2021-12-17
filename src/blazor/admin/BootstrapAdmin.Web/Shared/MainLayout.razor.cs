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
        private INavigations? NavigationsService { get; set; }

        [Inject]
        [NotNull]
        private IDicts? DictsService { get; set; }

        [Inject]
        [NotNull]
        private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        [Inject]
        [NotNull]
        private IUsers? UsersService { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? Context { get; set; }

        [Inject]
        [NotNull]
        private IBootstrapAdminService? SecurityService { get; set; }

        private string? Title { get; set; }

        private string? Footer { get; set; }

        private string? DisplayName { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userName = state.User.Identity?.Name;

            if (!string.IsNullOrEmpty(userName))
            {
                DisplayName = UsersService.GetDisplayName(userName);
                Context.UserName = userName;
                Context.DisplayName = DisplayName;

                MenuItems = NavigationsService.GetAllMenus(userName).ToAdminMenus();
            }

            Title = DictsService.GetWebTitle();
            Footer = DictsService.GetWebFooter();
        }

        private Task<bool> OnAuthorizing(string url) => SecurityService.AuhorizingNavigation(Context.UserName, url);
    }
}
