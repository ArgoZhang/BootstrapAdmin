using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 可编辑页面组件包含查询与数据表格
    /// </summary>
    public class EditPageBase<TItem> : BootstrapComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

#nullable disable
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public TItem QueryModel { get; set; }
#nullable restore

        /// <summary>
        /// 查询模板
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? QueryBody { get; set; }

        /// <summary>
        /// 查询按钮回调方法
        /// </summary>
        [Parameter]
        public Func<int, int, QueryData<TItem>>? OnQuery { get; set; }

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
        /// 按钮模板
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ButtonTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? TableFooter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? EditTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected Table<TItem>? Table { get; set; }

        /// <summary>
        /// 新建按钮回调方法
        /// </summary>
        [Parameter]
        public Func<TItem>? OnAdd { get; set; }

        /// <summary>
        /// 编辑按钮回调方法
        /// </summary>
        [Parameter]
        public Action<TItem>? OnEdit { get; set; }

        /// <summary>
        /// 保存按钮回调方法
        /// </summary>
        [Parameter]
        public Func<TItem, bool>? OnSave { get; set; }

        /// <summary>
        /// 删除按钮回调方法
        /// </summary>
        [Parameter]
        public Func<IEnumerable<TItem>, bool>? OnDelete { get; set; }

        /// <summary>
        /// 组件初始化方法
        /// </summary>
        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(Id)) throw new InvalidOperationException($"The property {nameof(Id)} can't set to Null");
        }

        /// <summary>
        /// 数据表格内删除按钮方法
        /// </summary>
        /// <param name="item"></param>
        protected void Delete(TItem item)
        {
            if (Table != null)
            {
                Table.SelectedItems.Clear();
                Table.SelectedItems.Add(item);
                Table.Delete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Edit(TItem item)
        {
            if (Table != null)
            {
                Table.SelectedItems.Clear();
                Table.SelectedItems.Add(item);
                Table.Edit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Query()
        {
            // 查询控件按钮触发此事件
            if (OnQuery != null && Table != null)
            {
                Table.Query(OnQuery.Invoke(1, Table.PageItems));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageItems"></param>
        /// <returns></returns>
        protected QueryData<TItem> QueryData(int pageIndex, int pageItems) => OnQuery?.Invoke(pageIndex, pageItems) ?? new QueryData<TItem>();
    }
}
