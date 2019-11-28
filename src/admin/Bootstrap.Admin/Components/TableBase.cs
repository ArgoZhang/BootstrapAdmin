using Bootstrap.Admin.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
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
        public bool ShowToolBar { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> Items { get; set; } = new TItem[0];

        /// <summary>
        /// 
        /// </summary>
        public Action AddCallback { get; set; } = new Action(() => { });
        /// <summary>
        /// 
        /// </summary>
        public Action EditCallback { get; set; } = new Action(() => { });

        /// <summary>
        /// 
        /// </summary>
        public void Delete()
        {

        }
    }
}
