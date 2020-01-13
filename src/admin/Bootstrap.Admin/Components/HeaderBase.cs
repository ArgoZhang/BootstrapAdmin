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
        /// 获得 网站标题
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        /// 获得 根模板页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout RootLayout { get; protected set; } = new DefaultLayout();

        /// <summary>
        /// 获得/设置 用户图标
        /// </summary>
        [Parameter]
        public string Icon { get; set; } = "";

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
    }
}
