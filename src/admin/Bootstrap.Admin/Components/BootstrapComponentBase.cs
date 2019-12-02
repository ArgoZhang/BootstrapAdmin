using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapComponentBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Admin")]
        protected AdminLayout Layout { get; set; } = new AdminLayout();

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="cate"></param>
        protected void ShowMessage(string title, string text, ToastCategory cate = ToastCategory.Success) => Layout.ShowMessage(title, text, cate);
    }
}
