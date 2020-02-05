using Bootstrap.Admin.Pages.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 可编辑页面组件包含查询与数据表格
    /// </summary>
    public class EditPageBase<TItem> : ComponentBase
    {
        /// <summary>
        /// 获得/设置 Id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

#nullable disable
        /// <summary>
        /// 获得/设置 QueryModel 实例
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
        public Func<QueryPageOptions, QueryData<TItem>>? OnQuery { get; set; }

        /// <summary>
        /// 获得/设置 TableHeader 实例
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? TableHeader { get; set; }

        /// <summary>
        /// 获得/设置 RowTemplate 实例
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? RowTemplate { get; set; }

        /// <summary>
        /// 获得/设置 按钮模板
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ButtonTemplate { get; set; }

        /// <summary>
        /// 获得/设置 表格 Toolbar 按钮模板
        /// </summary>
        [Parameter]
        public RenderFragment? TableToolbarTemplate { get; set; }

        /// <summary>
        /// 获得/设置 提示信息模板
        /// </summary>
        [Parameter]
        public RenderFragment? TableInfoTemplate { get; set; }

        /// <summary>
        /// 获得/设置 TableFooter 实例
        /// </summary>
        [Parameter]
        public RenderFragment? TableFooter { get; set; }

        /// <summary>
        /// 获得/设置 EditTemplate 实例
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? EditTemplate { get; set; }

        /// <summary>
        /// 获得/设置 SearchTemplate 实例
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? SearchTemplate { get; set; }

        /// <summary>
        /// 获得/设置 是否固定表头 默认为 false 不固定表头
        /// </summary>
        [Parameter]
        public bool FixedHeader { get; set; }

        /// <summary>
        /// 获得/设置 Table 实例
        /// </summary>
        protected Table<TItem>? Table { get; set; }

        /// <summary>
        /// 编辑数据弹窗 Title
        /// </summary>
        [Parameter]
        public string SubmitModalTitle { get; set; } = "";

        /// <summary>
        /// 新建按钮回调方法
        /// </summary>
        [Parameter]
        public Func<TItem> OnAdd { get; set; } = () => throw new InvalidOperationException($"The property {nameof(OnAdd)} can't be set to Null");

        /// <summary>
        /// 保存按钮回调方法
        /// </summary>
        [Parameter]
        public Func<TItem, bool> OnSave { get; set; } = item => false;

        /// <summary>
        /// 重置搜索条件回调方法
        /// </summary>
        [Parameter]
        public Action OnResetSearch { get; set; } = () => { };

        /// <summary>
        /// 删除按钮回调方法
        /// </summary>
        [Parameter]
        public Func<IEnumerable<TItem>, bool> OnDelete { get; set; } = item => false;

        /// <summary>
        /// 组件初始化方法
        /// </summary>
        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(Id)) throw new InvalidOperationException($"The property {nameof(Id)} can't be set to Null");
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
        /// 编辑方法
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
        /// 获得 Table 组件选择项目集合
        /// </summary>
        public IEnumerable<TItem> SelectedItems { get { return Table?.SelectedItems ?? new List<TItem>(); } }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected QueryData<TItem> QueryData(QueryPageOptions options) => OnQuery?.Invoke(options) ?? new QueryData<TItem>();
    }
}
