using Bootstrap.Admin.Models;
using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 侧边栏组件
    /// </summary>
    public class SideBarBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 侧边栏绑定 Model 实例
        /// </summary>
        [Parameter]
        public NavigatorBarModel Model { get; set; } = new NavigatorBarModel("");

        /// <summary>
        ///
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout RootLayout { get; protected set; } = new DefaultLayout();

        /// <summary>
        ///
        /// </summary>
        public void UpdateState()
        {
            StateHasChanged();
        }
    }
}
