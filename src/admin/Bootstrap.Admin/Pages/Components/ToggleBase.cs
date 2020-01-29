using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// Toggle 开关组件
    /// </summary>
    public class ToggleBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 组件高度
        /// </summary>
        [Parameter]
        public int Width { get; set; } = 120;

        /// <summary>
        /// 获得/设置 组件 On 时显示文本
        /// </summary>
        [Parameter]
        public string OnText { get; set; } = "展开";

        /// <summary>
        /// 获得/设置 组件 Off 时显示文本
        /// </summary>
        [Parameter]
        public string OffText { get; set; } = "收缩";

        /// <summary>
        /// 获得/设置 组件是否处于 On 状态 默认为 Off 状态
        /// </summary>
        [Parameter]
        public bool Value { get; set; } = false;

        /// <summary>
        /// 获得/设置 Value 值改变时回调事件
        /// </summary>
        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        /// <summary>
        /// 获得/设置 Value 值改变时回调事件
        /// </summary>
        protected void ToggleClick()
        {
            Value = !Value;
            ValueChanged.InvokeAsync(Value);
        }
    }
}
