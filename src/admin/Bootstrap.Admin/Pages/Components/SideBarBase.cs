using Bootstrap.Admin.Models;
using Bootstrap.Admin.Pages.Shared;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
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
        /// 获得 根模板页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout? RootLayout { get; protected set; }

        /// <summary>
        /// 获得/设置 用户显示名称
        /// </summary>
        [Parameter]
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// 获得/设置 用户显示名称改变事件回调方法
        /// </summary>
        [Parameter]
        public EventCallback<string> DisplayNameChanged { get; set; }

        /// <summary>
        /// 获得/设置 网站标题
        /// </summary>
        [Parameter]
        public string WebTitle { get; set; } = "";

        /// <summary>
        /// 获得/设置 网站标题改变事件回调方法
        /// </summary>
        [Parameter]
        public EventCallback<string> WebTitleChanged { get; set; }
    }
}
