using Bootstrap.Security;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class NavItemBase : BootstrapComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public BootstrapMenu Menu { get; set; } = new BootstrapMenu();
    }
}
