using Bootstrap.Admin.Models;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 侧边栏组件
    /// </summary>
    public class SideBarBase : BootstrapComponentBase
    {
        /// <summary>
        /// 获得/设置 侧边栏绑定 Model 实例
        /// </summary>
        [Parameter]
        public NavigatorBarModel Model { get; set; } = new NavigatorBarModel("");

        /// <summary>
        /// 视图更新方法
        /// </summary>
        public void Update(NavigatorBarModel model)
        {
            Model = model;
            StateHasChanged();
        }
    }
}
