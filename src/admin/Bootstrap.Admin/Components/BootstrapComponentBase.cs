using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Admin")]
        protected AdminLayout Layout { get; set; } = new AdminLayout();
    }
}
