using Bootstrap.Security;

namespace Bootstrap.Admin.Blazor.Components
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
