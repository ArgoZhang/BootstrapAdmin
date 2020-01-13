using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SectionBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowBackground { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int LockScreenPeriod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string ShowCardTitle { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
