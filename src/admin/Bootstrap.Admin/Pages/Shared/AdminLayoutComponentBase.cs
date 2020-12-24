using Bootstrap.Admin.Pages.Shared;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// AdminLayout 布局类
    /// </summary>
    public class AdminLayoutComponentBase : LayoutComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public TabSet? TabSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout RootLayout { get; protected set; } = new DefaultLayout();
    }
}
