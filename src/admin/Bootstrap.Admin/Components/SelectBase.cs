using Microsoft.AspNetCore.Components;
using System;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// Select 组件基类
    /// </summary>
    public class SelectBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string PlaceHolder { get; set; } = "请选择 ...";

        /// <summary>
        /// 当前选择项实例
        /// </summary>
        public SelectItemBase? SelectedItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int SelectedValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<int> SelectedValueChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<SelectItemBase>? ClickItemCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<SelectItemBase>? ActiveChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ClickItemCallback = UpdateItem;
                ActiveChanged = UpdateItem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected void UpdateItem(SelectItemBase item)
        {
            SelectedItem = item;
            StateHasChanged();
        }
    }
}
