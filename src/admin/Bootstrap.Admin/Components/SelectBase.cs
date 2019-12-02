using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// Select 组件基类
    /// </summary>
    public class SelectBase<TItem> : ComponentBase
    {
        /// <summary>
        /// 获得/设置 控件 ID
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 获得/设置 背景显示文字
        /// </summary>
        [Parameter]
        public string PlaceHolder { get; set; } = "请选择 ...";

        /// <summary>
        /// 当前选择项实例
        /// </summary>
        public SelectedItem SelectedItem { get; set; } = new SelectedItem();

        /// <summary>
        /// 获得/设置 绑定数据集
        /// </summary>
        [Parameter]
        public List<SelectedItem> Items { get; set; } = new List<SelectedItem>();

#nullable disable
        private TItem _value;
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public TItem SelectedValue
        {
            get { return _value; }
            set
            {
                _value = value;
                Items.ForEach(t =>
                {
                    t.Active = t.Value == _value.ToString();
                    if (t.Active) SelectedItem = t;
                });
            }
        }
#nullable restore

        ///<summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<TItem> SelectedValueChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            if (!SelectedItem.Active)
            {
                SelectedItem = Items.FirstOrDefault(item => item.Active) ?? Items.First();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Action<SelectedItem> SelectedItemChanged { get; set; } = new Action<SelectedItem>(v => { });

        /// <summary>
        /// 
        /// </summary>
        public void ItemClickCallback(SelectedItem item)
        {
            SelectedItemChanged(item);
            SelectedItem = item;
            StateHasChanged();
        }
    }
}
