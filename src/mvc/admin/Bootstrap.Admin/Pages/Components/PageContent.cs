using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// PageContent 网页组件
    /// </summary>
    public class PageContent : ComponentBase
    {
        /// <summary>
        /// 获得/设置 组件名字
        /// </summary>
        [Parameter]
        public string Name { get; set; } = "";

        /// <summary>
        /// 渲染组件方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var name = Name.Replace("/", ".");
            if (!string.IsNullOrEmpty(name))
            {
                var t = Type.GetType($"Bootstrap.Admin.Pages.Views.{name}");
                if (t != null)
                {

                    builder.OpenComponent(0, t);
                    builder.CloseComponent();
                }
                else
                {
                    builder.OpenElement(0, "h6");
                    builder.AddContent(1, "正在玩命开发中... 敬请期待");
                    builder.CloseElement();
                }
            }
        }
    }
}
