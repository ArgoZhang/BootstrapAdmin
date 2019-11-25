using Bootstrap.Admin.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SideBarBase : BootstrapComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public NavigatorBarModel? Model { get; set; }
    }
}
