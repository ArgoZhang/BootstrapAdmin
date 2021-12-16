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
        public EventCallback<MouseEventArgs> OnClick { get; set; }
    }
}
