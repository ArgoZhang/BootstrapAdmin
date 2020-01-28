using Bootstrap.Security;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class NavItemBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public BootstrapMenu Menu { get; set; } = new BootstrapMenu();
    }
}
