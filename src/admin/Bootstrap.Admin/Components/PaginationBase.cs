using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 分页组件基类
    /// </summary>
    public class PaginationBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 页码总数
        /// </summary>
        [Parameter]
        public int PageCount { get; set; } = 0;

        /// <summary>
        /// 获得/设置 每页显示数据数量
        /// </summary>
        [Parameter]
        public int PageItems { get; set; } = 20;

        /// <summary>
        /// 获得/设置 数据总数
        /// </summary>
        [Parameter]
        public int ItemsCount { get; set; } = 0;

        /// <summary>
        /// 获得/设置 当前页码
        /// </summary>
        [Parameter]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<int> ClickPageCallback { get; set; } = new Action<int>(i => { });

        /// <summary>
        /// 
        /// </summary>
        public Action PageItemsChangeCallback { get; set; } = new Action(() => { });

        /// <summary>
        /// 
        /// </summary>
        protected void MovePrev()
        {
            if (PageIndex > 1) ClickPageCallback(PageIndex - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void MoveNext()
        {
            if (PageIndex < PageCount) ClickPageCallback(PageIndex + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageItems"></param>
        protected void ClickItem(int pageItems)
        {
            PageItems = pageItems;
            PageItemsChangeCallback();
        }
    }
}
