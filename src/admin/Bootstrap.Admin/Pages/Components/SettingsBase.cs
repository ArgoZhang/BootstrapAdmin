using Bootstrap.Admin.Models;
using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Pages.Admin.Components
{
    /// <summary>
    /// 网站设置组件
    /// </summary>
    public class SettingsBase : ComponentBase
    {
        /// <summary>
        /// 获得 SettingsModel 实例
        /// </summary>
        protected SettingsModel? Model { get; set; }

        /// <summary>
        /// 获得/设置 默认母版页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout? RootLayout { get; protected set; }

        /// <summary>
        /// 设置参数方法
        /// </summary>
        protected override void OnParametersSet()
        {
            Model = new SettingsModel(RootLayout?.UserName);
        }
    }
}
