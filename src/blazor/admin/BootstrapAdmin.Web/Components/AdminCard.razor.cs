using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AdminCard
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? AuthorizeKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [EditorRequired]
        [NotNull]
        public string? HeaderText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Inject]
        [NotNull]
        private INavigation? NavigationService { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? AppContext { get; set; }

        [Inject]
        [NotNull]
        private NavigationManager? NavigationManager { get; set; }

        private Task<bool> OnQueryCondition(string name)
        {
            var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

            return Task.FromResult(NavigationService.AuthorizationBlock(AppContext.UserName, url, name));
        }
    }
}
