using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class PageContent : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Name { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var name = Name.SpanSplit("/").LastOrDefault();
            if (!string.IsNullOrEmpty(name))
            {
                var t = Type.GetType($"Bootstrap.Admin.Pages.Admin.{name}");
                if (t != null)
                {
                    builder.OpenComponent(0, t);
                    builder.CloseComponent();
                }
            }
        }
    }
}
