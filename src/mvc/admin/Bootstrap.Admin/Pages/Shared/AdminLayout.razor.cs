using Bootstrap.Admin.Pages.Components;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Shared
{
    /// <summary>
    /// AdminLayout 布局类
    /// </summary>
    public partial class AdminLayout
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
