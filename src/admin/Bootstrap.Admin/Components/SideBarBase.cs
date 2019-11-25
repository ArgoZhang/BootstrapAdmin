using Bootstrap.Admin.Models;
using Microsoft.AspNetCore.Components;

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
        [Parameter]
        public NavigatorBarModel Model { get; set; } = new NavigatorBarModel("");
    }
}
