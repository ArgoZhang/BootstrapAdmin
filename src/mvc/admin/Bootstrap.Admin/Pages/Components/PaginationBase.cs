using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 分页组件基类
    /// </summary>
    public class PaginationBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int TotalCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int PageItems { get; set; }

        /// <summary>
        /// 获得/设置 页码总数
        /// </summary>
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling(TotalCount * 1.0 / PageItems); ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Action<int, int> OnPageClick { get; set; } = new Action<int, int>((pageIndex, pageItems) => { });

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Action<int> OnPageItemsChange { get; set; } = new Action<int>((pageItems) => { });

        /// <summary>
        /// 
        /// </summary>
        protected void MovePrev()
        {
            if (PageIndex > 1) OnPageClick(PageIndex - 1, PageItems);
            else OnPageClick(PageCount, PageItems);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void MoveNext()
        {
            if (PageIndex < PageCount) OnPageClick(PageIndex + 1, PageItems);
            else OnPageClick(1, PageItems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageItems"></param>
        protected void ClickItem(int pageItems)
        {
            PageItems = pageItems;
            OnPageItemsChange(PageItems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<int> GetPages()
        {
            var pages = new List<int>() { 20, 40, 80, 100, 200 };
            var ret = new List<int>();
            for (int i = 0; i < pages.Count; i++)
            {
                ret.Add(pages[i]);
                if (pages[i] >= TotalCount) break;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        protected int StartPageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected int EndPageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnParametersSet()
        {
            // 计算 分页开始页码与结束页码
            StartPageIndex = Math.Max(1, PageIndex - 4);
            EndPageIndex = Math.Min(PageCount, PageIndex + 5);
        }
    }
}
