using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// Select 组件基类
    /// </summary>
    public class SelectBase<TItem> : ValidateInputBase<TItem>
    {
        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string ColumnClass { get; set; } = "col-sm-6";

        /// <summary>
        /// 当前选择项实例
        /// </summary>
        public SelectedItem SelectedItem { get; set; } = new SelectedItem();

        /// <summary>
        /// 获得/设置 绑定数据集
        /// </summary>
        [Parameter]
        public List<SelectedItem> Items { get; set; } = new List<SelectedItem>();

        /// <summary>
        /// 
        /// </summary>
        protected override void OnParametersSet()
        {
            Items.ForEach(t =>
            {
                t.Active = t.Value == Value?.ToString();
                if (t.Active) SelectedItem = t;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (!SelectedItem.Active)
            {
                SelectedItem = Items.FirstOrDefault(item => item.Active) ?? Items.First();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Action<SelectedItem>? SelectedItemChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void ItemClickCallback(SelectedItem item)
        {
            SelectedItem = item;
            CurrentValueAsString = item.Value;
        }
    }
}
