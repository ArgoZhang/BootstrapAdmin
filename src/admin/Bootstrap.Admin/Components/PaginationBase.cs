using Bootstrap.Admin.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

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
        /// 
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
        public Action<int>? ClickPageCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected void MovePrev()
        {
            if (PageIndex > 1) ClickPageCallback?.Invoke(PageIndex - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void MoveNext()
        {
            if (PageIndex < PageCount) ClickPageCallback?.Invoke(PageIndex + 1);
        }
    }
}
