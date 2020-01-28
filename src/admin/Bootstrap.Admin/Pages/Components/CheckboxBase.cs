using Microsoft.AspNetCore.Components;
using System;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// Checkbox 组件基类
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class CheckboxBase<TItem> : ComponentBase
    {
#nullable disable
        /// <summary>
        /// 获得/设置 数据绑定项
        /// </summary>
        [Parameter]
        public TItem Item { get; set; }
#nullable restore

        /// <summary>
        /// 获得/设置 显示文本
        /// </summary>
        [Parameter]
        public string Text { get; set; } = "";

        /// <summary>
        /// 获得/设置 是否被选中
        /// </summary>
        protected bool Checked { get; set; }

        /// <summary>
        /// 勾选回调方法
        /// </summary>
        [Parameter]
        public Action<TItem, bool> ToggleCallback { get; set; } = new Action<TItem, bool>((v, c) => { });

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, CheckBoxState> SetCheckCallback { get; set; } = new Func<TItem, CheckBoxState>(item => CheckBoxState.UnChecked);

        /// <summary>
        /// 
        /// </summary>
        protected override void OnParametersSet()
        {
            State = SetCheckCallback(Item);
            Checked = State == CheckBoxState.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public CheckBoxState State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string RenderStateCss()
        {
            var ret = "form-checkbox";
            switch (State)
            {
                case CheckBoxState.Mixed:
                    ret = "form-checkbox is-indeterminate";
                    break;
                case CheckBoxState.Checked:
                    ret = "form-checkbox is-checked";
                    break;
                case CheckBoxState.UnChecked:
                    break;
            }
            return ret;
        }
    }
}
