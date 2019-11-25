using Bootstrap.Admin.Extensions;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class TabBase : BootstrapComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Url { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Icon { get; set; } = "";

        private bool closeTab;
        /// <summary>
        /// 
        /// </summary>
        protected void CloseTab()
        {
            closeTab = true;
            Layout?.CloseTab(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ClickTab()
        {
            if (!closeTab) NavigationManager?.NavigateTo(Url);
            else closeTab = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        public void Init(BootstrapMenu menu)
        {
            Title = menu.Name;
            Url = menu.Url.ToBlazorMenuUrl();
            Icon = menu.Icon;
            Id = menu.Id;
        }
    }
}
