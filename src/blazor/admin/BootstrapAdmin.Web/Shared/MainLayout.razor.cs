using BootstrapAdmin.DataAccess.Services;

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
        private IMenu? MenuService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            MenuItems = MenuService.GetAdminMenusByUser("Admin");
        }
    }
}
