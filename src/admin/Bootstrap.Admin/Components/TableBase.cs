using Bootstrap.Admin.Extensions;
using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public const int DefaultPageItems = 20;

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
        /// 按钮模板
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ButtonTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? EditTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? TableFooter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected IEnumerable<TItem> Items { get; set; } = new TItem[0];

        /// <summary>
        /// 
        /// </summary>
        public List<TItem> SelectedItems { get; } = new List<TItem>();

        /// <summary>
        /// 获得/设置 是否显示行号
        /// </summary>
        [Parameter]
        public bool ShowLineNo { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否显示选择列 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowCheckbox { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否显示按钮列 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowButtons { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否显示工具栏 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowToolBar { get; set; } = true;

        /// <summary>
        /// 获得/设置 按钮列 Header 文本 默认为 操作
        /// </summary>
        [Parameter]
        public string ButtonTemplateHeaderText { get; set; } = "操作";

        /// <summary>
        /// 点击翻页回调方法
        /// </summary>
        [Parameter]
        public Func<int, int, QueryData<TItem>>? OnQuery { get; set; }

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
        public int PageItems { get; set; } = DefaultPageItems;

        /// <summary>
        /// 确认删除弹窗
        /// </summary>
        protected Modal? ConfirmModal { get; set; }

#nullable disable
        /// <summary>
        /// 
        /// </summary>
        protected TItem EditModel { get; set; }
#nullable restore

        /// <summary>
        /// 编辑数据弹窗
        /// </summary>
        protected SubmitModal<TItem>? EditModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            if (OnAdd != null) EditModel = OnAdd.Invoke();
            if (OnQuery != null)
            {
                var queryData = OnQuery.Invoke(1, DefaultPageItems);
                Items = queryData.Items;
                TotalCount = queryData.TotalCount;
            }
        }

        /// <summary>
        /// 
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
        /// 
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
        /// 
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
        /// 
        /// </summary>
        /// <param name="item"></param>
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
        /// 
        /// </summary>
        public void Add()
        {
            if (OnAdd != null) EditModel = OnAdd.Invoke();
            SelectedItems.Clear();
            EditModal?.Toggle();
        }

        /// <summary>
        /// Toast 组件实例
        /// </summary>
        protected Toast? Toast { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="cate"></param>
        protected void ShowMessage(string title, string text, ToastCategory cate = ToastCategory.Success) => Toast?.ShowMessage(title, text, cate);

        /// <summary>
        /// 
        /// </summary>
        public void Edit()
        {
            if (SelectedItems.Count == 1)
            {
                EditModel = SelectedItems[0].Clone();
                EditModal?.Toggle();
            }
            else
            {
                ShowMessage("编辑数据", "请选择一个要编辑的数据", ToastCategory.Information);
            }
        }

        /// <summary>
        /// 更新查询数据
        /// </summary>
        /// <param name="queryData"></param>
        public void Query(QueryData<TItem> queryData)
        {
            SelectedItems.Clear();
            PageIndex = queryData.PageIndex;
            Items = queryData.Items;
            TotalCount = queryData.TotalCount;
            StateHasChanged();
        }

        private void Query()
        {
            if (OnQuery != null) Query(OnQuery.Invoke(PageIndex, PageItems));
        }

        /// <summary>
        /// 
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
    }
}
