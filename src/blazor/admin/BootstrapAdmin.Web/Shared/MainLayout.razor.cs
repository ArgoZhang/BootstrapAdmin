using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;

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
        private IUsers? UsersService { get; set; }

        [Inject]
        [NotNull]
        private IUsers? UsersService { get; set; }

        private string? Title { get; set; }

        private string? Footer { get; set; }

        private string? DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            MenuItems = NavigationsService.RetrieveAllMenus("Admin").ToAdminMenus();

            Title = DictsService.GetWebTitle();
            Footer = DictsService.GetWebFooter();
            DisplayName = UsersService.GetDisplayName();
        }
    }
}
