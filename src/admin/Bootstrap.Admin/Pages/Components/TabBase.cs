using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.Admin.Pages.Shared;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    ///
    /// </summary>
    public class TabBase : ComponentBase
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

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public bool Active { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Admin")]
        public AdminLayout Layout { get; protected set; } = new AdminLayout();

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        private bool closeTab;
        /// <summary>
        ///
        /// </summary>
        protected async Task CloseTab()
        {
            closeTab = true;
            if (Layout.TabSet != null) await Layout.TabSet.CloseTab(Id);
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
            Active = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="val"></param>
        public void SetActive(bool val) => Active = val;
    }
}
