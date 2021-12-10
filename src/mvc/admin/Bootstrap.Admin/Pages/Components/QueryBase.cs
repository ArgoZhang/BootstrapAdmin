using Microsoft.AspNetCore.Components;
using System;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 查询组件
    /// </summary>
    public class QueryBase<TItem> : ComponentBase
    {
        private readonly string _defaultTitle = "查询条件";
        private readonly string _defaultText = "查询";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 查询组件标题 默认为 查询条件
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        /// 查询按钮显示文字 默认为 查询
        /// </summary>
        [Parameter]
        public string Text { get; set; } = "";

        /// <summary>
        /// 查询组件模板
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ChildContent { get; set; }

#nullable disable
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public TItem QueryModel { get; set; }
#nullable restore

        /// <summary>
        /// 查询按钮回调方法
        /// </summary>
        [Parameter]
        public Action? OnQuery { get; set; }

        /// <summary>
        /// 参数设置方法
        /// </summary>
        protected override void OnParametersSet()
        {
            if (string.IsNullOrEmpty(Title)) Title = _defaultTitle;
            if (string.IsNullOrEmpty(Text)) Text = _defaultText;
        }
    }
}
