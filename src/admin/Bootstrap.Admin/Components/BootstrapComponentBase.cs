using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapComponentBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Admin")]
        protected AdminLayout Layout { get; set; } = new AdminLayout();

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Default")]
        protected DefaultLayout RootLayout { get; set; } = new DefaultLayout();

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
    }
}
