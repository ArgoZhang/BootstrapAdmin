using Microsoft.AspNetCore.Components.Web;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LinkButton
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Url { get; set; } = "#";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? Img { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Placement TooltipPlacement { get; set; } = Placement.Top;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        private bool Prevent => Url.StartsWith('#');
    }
}
