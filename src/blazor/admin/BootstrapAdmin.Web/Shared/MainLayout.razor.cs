using BootstrapAdmin.DataAccess.Core;
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

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            MenuItems = NavigationsService.RetrieveAllMenus("Admin").ToAdminMenus();
        }
    }
}
