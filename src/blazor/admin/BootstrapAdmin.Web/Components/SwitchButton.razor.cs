using Microsoft.AspNetCore.Components.Web;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SwitchButton
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? OnText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? OffText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ToggleState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<bool> ToggleStateChanged { get; set; }

        private async Task Toggle()
        {
            ToggleState = !ToggleState;
            if (ToggleStateChanged.HasDelegate)
            {
                await ToggleStateChanged.InvokeAsync(ToggleState);
            }
        }

        private string? GetText() => ToggleState ? OnText : OffText;
    }
}
