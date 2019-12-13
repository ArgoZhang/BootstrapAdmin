using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    ///
    /// </summary>
    public class HeaderBase : ComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout RootLayout { get; protected set; } = new DefaultLayout();

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string Icon { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public void UpdateDisplayName()
        {
            StateHasChanged();
        }
    }
}
