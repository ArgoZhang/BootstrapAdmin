using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.Admin.Pages.Shared;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
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
        public string WebTitle { get; set; } = "";

        /// <summary>
        /// 获得/设置 网站标题改变事件回调方法
        /// </summary>
        [Parameter]
        public EventCallback<string> WebTitleChanged { get; set; }

        /// <summary>
        /// 获得 根模板页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        protected DefaultLayout? RootLayout { get; set; }

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

        /// <summary>
        /// 获得/设置 是否显示 Blazor MVC 切换图标
        /// </summary>
        protected bool EnableBlazor { get; set; }

        /// <summary>
        /// 参数赋值方法
        /// </summary>
        public override System.Threading.Tasks.Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            EnableBlazor = DictHelper.RetrieveEnableBlazor();
            return base.SetParametersAsync(ParameterView.Empty);
        }
    }
}
