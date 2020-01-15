using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 条件输出组件
    /// </summary>
    public class ConditionComponent : ComponentBase
    {
        /// <summary>
        /// 获得/设置 是否显示 默认 true 显示
        /// </summary>
        [Parameter]
        public bool Inverse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout? RootLayout { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // TODO: 改造成通用的条件输出组件
            // 目前内置了 IsDemo
            var render = RootLayout?.Model.IsDemo ?? true;
            if (Inverse) render = !render;
            if (render) builder.AddContent(0, ChildContent);
        }
    }
}
