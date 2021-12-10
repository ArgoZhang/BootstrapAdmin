using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// Dropdown 组件
    /// </summary>
    public class DropdownBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 绑定数据集合
        /// </summary>
        [Parameter]
        public IEnumerable<SelectedItem> Items { get; set; } = new SelectedItem[0];

        /// <summary>
        /// 获得/设置 选中项实例
        /// </summary>
        [Parameter]
        public SelectedItem Value { get; set; } = new SelectedItem();

        /// <summary>
        /// 获得/设置 选中项改变回调方法
        /// </summary>
        [Parameter]
        public EventCallback<SelectedItem> ValueChanged { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected void OnClick(SelectedItem item)
        {
            Value = item;
            if (ValueChanged.HasDelegate) ValueChanged.InvokeAsync(Value);
        }
    }
}
