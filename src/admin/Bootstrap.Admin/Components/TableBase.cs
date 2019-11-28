using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 表格组件类
    /// </summary>
    public class TableBase<TItem> : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? TableHeader { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? RowTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? TableFooter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> Items { get; set; } = new TItem[0];

        /// <summary>
        /// 数据
        /// </summary>
        [Parameter]
        public int ItemsCount { get; set; } = 0;
    }
}
