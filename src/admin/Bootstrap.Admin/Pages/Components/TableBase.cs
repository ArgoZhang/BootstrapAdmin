using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.Admin.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 表格组件类
    /// </summary>
    public class TableBase<TItem> : ComponentBase
    {
        /// <summary>
        /// 获得 IJSRuntime 实例
        /// </summary>
        [Inject]
        [NotNull]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 每页数据数量 默认 20 行
        /// </summary>
        protected const int DefaultPageItems = 20;

        /// <summary>
        /// 获得/设置 组件 Id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

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
        /// 获得/设置 按钮模板 实例
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ButtonTemplate { get; set; }

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
        /// 获得/设置 表格 Toolbar 按钮模板
        /// </summary>
        [Parameter]
        public RenderFragment? TableToolbarTemplate { get; set; }

        /// <summary>
        /// 获得/设置 TableFooter 实例
        /// </summary>
        [Parameter]
        public RenderFragment? TableFooter { get; set; }

        /// <summary>
        /// 获得/设置 是否固定表头 默认为 false 不固定表头
        /// </summary>
        [Parameter]
        public bool FixedHeader { get; set; }

        /// <summary>
        /// 获得/设置 是否自适应高度 默认为 false 不自适应高度
        /// </summary>
        [Parameter]
        public bool AutoHeight { get; set; }

        /// <summary>
        /// 获得/设置 是否显示搜索框 默认为 false 不显示搜索框
        /// </summary>
        [Parameter]
        public bool ShowSearch { get; set; }

        /// <summary>
        /// 获得/设置 是否显示高级搜索按钮 默认显示
        /// </summary>
        [Parameter]
        public bool ShowAdvancedSearch { get; set; } = true;

        /// <summary>
        /// 获得/设置 数据集合
        /// </summary>
        protected IEnumerable<TItem> Items { get; set; } = new TItem[0];

        /// <summary>
        /// 获得/设置 已选择的数据集合
        /// </summary>
        public List<TItem> SelectedItems { get; } = new List<TItem>();

        /// <summary>
        /// 获得/设置 是否显示行号
        /// </summary>
        [Parameter]
        public bool ShowLineNo { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否显示选择列 默认为 false
        /// </summary>
        [Parameter]
        public bool ShowCheckbox { get; set; }

        /// <summary>
        /// 获得/设置 是否显示按钮列 默认为 false
        /// </summary>
        [Parameter]
        public bool ShowDefaultButtons { get; set; }

        /// <summary>
        /// 获得/设置 是否显示表脚 默认为 false
        /// </summary>
        [Parameter]
        public bool ShowFooter { get; set; }

        /// <summary>
        /// 获得/设置 是否显示扩展按钮 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowExtendButtons { get; set; }

        /// <summary>
        /// 获得/设置 是否显示刷新按钮 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowRefresh { get; set; }

        /// <summary>
        /// 获得/设置 是否分页组件 默认为 false
        /// </summary>
        [Parameter]
        public bool ShowPagination { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否显示工具栏 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowToolBar { get; set; }

        /// <summary>
        /// 获得/设置 按钮列 Header 文本 默认为 操作
        /// </summary>
        [Parameter]
        public string ButtonTemplateHeaderText { get; set; } = "操作";

        /// <summary>
        /// 点击翻页回调方法
        /// </summary>
        [Parameter]
        public Func<QueryPageOptions, QueryData<TItem>>? OnQuery { get; set; }

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
        /// 表头排序时回调方法
        /// </summary>
        [Parameter]
        public Action<string, SortOrder> OnSort { get; set; } = new Action<string, SortOrder>((name, order) => { });

        /// <summary>
        /// 删除按钮回调方法
        /// </summary>
        [Parameter]
        public Func<IEnumerable<TItem>, bool>? OnDelete { get; set; }

        /// <summary>
        /// 获得/设置 每页数据数量
        /// </summary>
        [Parameter]
        public int PageItems { get; set; } = DefaultPageItems;

#nullable disable
        /// <summary>
        /// 获得/设置 EditModel 实例
        /// </summary>
        [Parameter]
        public TItem EditModel { get; set; }

        /// <summary>
        /// 获得/设置 QueryModel 实例
        /// </summary>
        [Parameter]
        public TItem QueryModel { get; set; }
#nullable restore

        /// <summary>
        /// 编辑数据弹窗 Title
        /// </summary>
        [Parameter]
        public string SubmitModalTitle { get; set; } = "";

        /// <summary>
        /// 编辑数据弹窗
        /// </summary>
        protected SubmitModal<TItem>? EditModal { get; set; }

        /// <summary>
        /// 确认删除弹窗
        /// </summary>
        protected Modal? ConfirmModal { get; set; }

        /// <summary>
        /// 高级查询弹窗
        /// </summary>
        protected Modal? SearchModal { get; set; }

        /// <summary>
        /// 获得/设置 数据总条目
        /// </summary>
        protected int TotalCount { get; set; }

        /// <summary>
        /// 获得/设置 当前页码
        /// </summary>
        protected int PageIndex { get; set; } = 1;

        /// <summary>
        /// 获得/设置 当前排序字段名称
        /// </summary>
        protected string SortName { get; set; } = "";

        /// <summary>
        /// 获得/设置 当前排序规则
        /// </summary>
        protected SortOrder SortOrder { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            OnSort = new Action<string, SortOrder>((sortName, sortOrder) =>
            {
                (SortName, SortOrder) = (sortName, sortOrder);
                Query();
            });
            if (EditModel == null && OnAdd != null) EditModel = OnAdd.Invoke();
            if (OnQuery != null)
            {
                var queryData = OnQuery(new QueryPageOptions() { PageItems = DefaultPageItems, SearchText = SearchText, SortName = SortName, SortOrder = SortOrder });
                Items = queryData.Items;
                TotalCount = queryData.TotalCount;
            }
        }

        /// <summary>
        /// OnAfterRender 方法
        /// </summary>
        protected override void OnAfterRender(bool firstRender)
        {
            // 调用客户端脚本
            JSRuntime.InitTableAsync(RetrieveId(), firstRender);
        }

        /// <summary>
        /// 点击页码调用此方法
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageItems"></param>
        protected void PageClick(int pageIndex, int pageItems)
        {
            if (pageIndex != PageIndex)
            {
                PageIndex = pageIndex;
                PageItems = pageItems;
                Query();
            }
        }

        /// <summary>
        /// 每页记录条数变化是调用此方法
        /// </summary>
        protected void PageItemsChange(int pageItems)
        {
            if (OnQuery != null)
            {
                PageIndex = 1;
                PageItems = pageItems;
                Query();
            }
        }

        /// <summary>
        /// 选择框点击时调用此方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="check"></param>
        protected void ToggleCheck(TItem item, bool check)
        {
            if (item == null)
            {
                SelectedItems.Clear();
                if (check) SelectedItems.AddRange(Items);
            }
            else
            {
                if (check) SelectedItems.Add(item);
                else SelectedItems.Remove(item);
            }
            StateHasChanged();
        }

        /// <summary>
        /// 表头 CheckBox 状态更新方法
        /// </summary>
        /// <returns></returns>
        protected CheckBoxState CheckState(TItem item)
        {
            var ret = CheckBoxState.UnChecked;
            if (SelectedItems.Count > 0)
            {
                ret = SelectedItems.Count == Items.Count() ? CheckBoxState.Checked : CheckBoxState.Mixed;
            }
            return ret;
        }

        /// <summary>
        /// 新建按钮方法
        /// </summary>
        public void Add()
        {
            if (OnAdd != null) EditModel = OnAdd.Invoke();
            SelectedItems.Clear();
            EditModal?.Toggle();
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="cate"></param>
        protected void ShowMessage(string title, string text, ToastCategory cate = ToastCategory.Success) => JSRuntime?.ShowToast(title, text, cate);

        /// <summary>
        /// 编辑按钮方法
        /// </summary>
        public void Edit()
        {
            if (SelectedItems.Count == 1)
            {
#nullable disable
                EditModel = SelectedItems[0].Clone();
#nullable restore
                EditModal?.Toggle();
            }
            else
            {
                ShowMessage("编辑数据", "请选择一个要编辑的数据", ToastCategory.Information);
            }
        }

        /// <summary>
        /// 查询按钮调用此方法
        /// </summary>
        public void Query()
        {
            if (OnQuery != null)
            {
                SelectedItems.Clear();
                var queryData = OnQuery(new QueryPageOptions()
                {
                    PageIndex = PageIndex,
                    PageItems = PageItems,
                    SearchText = SearchText,
                    SortOrder = SortOrder,
                    SortName = SortName
                });
                Items = queryData.Items;
                PageIndex = queryData.PageIndex;
                TotalCount = queryData.TotalCount;
                StateHasChanged();
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="context"></param>
        protected void Save(EditContext context)
        {
            var valid = OnSave?.Invoke((TItem)context.Model) ?? false;
            if (valid)
            {
                EditModal?.Toggle();
                Query();
            }
            ShowMessage("保存数据", "保存数据" + (valid ? "成功" : "失败"), valid ? ToastCategory.Success : ToastCategory.Error);
        }

        /// <summary>
        /// 删除按钮方法
        /// </summary>
        public void Delete()
        {
            if (SelectedItems.Count > 0)
            {
                ConfirmModal?.Toggle();
            }
            else
            {
                ShowMessage("删除数据", "请选择要删除的数据", ToastCategory.Information);
            }
        }

        /// <summary>
        /// 确认删除方法
        /// </summary>
        public void Confirm()
        {
            var result = OnDelete?.Invoke(SelectedItems) ?? false;
            if (result)
            {
                ConfirmModal?.Toggle();
                Query();
            }
            ShowMessage("删除数据", "删除数据" + (result ? "成功" : "失败"), result ? ToastCategory.Success : ToastCategory.Error);
        }

        /// <summary>
        /// 获取 Id 字符串
        /// </summary>
        protected string RetrieveId() => $"{Id}_table";

        /// <summary>
        /// 重置搜索按钮回调方法
        /// </summary>
        [Parameter]
        public Action? OnResetSearch { get; set; }

        /// <summary>
        /// 重置查询方法
        /// </summary>
        protected void ResetSearchClick()
        {
            OnResetSearch?.Invoke();
            SearchClick();
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        protected void SearchClick()
        {
            // 查询控件按钮触发此事件
            PageIndex = 1;
            Query();
        }

        /// <summary>
        /// 高级查询按钮点击时调用此方法
        /// </summary>
        protected void AdvancedSearchClick()
        {
            // 弹出高级查询弹窗
            SearchModal?.Toggle();
        }

        /// <summary>
        /// 获得/设置 搜索关键字
        /// </summary>
        [Parameter]
        public string SearchText { get; set; } = "";

        /// <summary>
        /// 获得/设置 搜索关键字改变事件
        /// </summary>
        [Parameter]
        public EventCallback<string> SearchTextChanged { get; set; }

        /// <summary>
        /// 重置搜索按钮调用此方法
        /// </summary>
        protected void ClearSearchClick()
        {
            SearchText = "";
            Query();
        }
    }
}
