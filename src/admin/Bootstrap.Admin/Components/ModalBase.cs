using Bootstrap.Admin.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 模态框组件类
    /// </summary>
    public class ModalBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public RenderFragment? ModalBody { get; set; }

        /// <summary>
        /// 获得/设置 弹窗 Footer 代码块
        /// </summary>
        [Parameter]
        public RenderFragment? ModalFooter { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "未设置";

        /// <summary>
        /// 获得/设置 是否允许点击后台关闭弹窗 默认为 false
        /// </summary>
        [Parameter]
        public bool Backdrop { get; set; }

        /// <summary>
        /// 获得/设置 弹窗大小
        /// </summary>
        [Parameter]
        public ModalSize Size { get; set; }

        /// <summary>
        /// 获得/设置 是否垂直居中 默认为 true
        /// </summary>
        [Parameter]
        public bool IsCentered { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否显示 Footer 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowFooter { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                JSRuntime.InitModal(Id);
            }
        }

        /// <summary>
        /// OnParametersSet 方法
        /// </summary>
        protected override void OnParametersSet()
        {
            if (string.IsNullOrEmpty(Id)) throw new InvalidOperationException("Modal Component Id property must be set");
        }

        /// <summary>
        /// 输出窗口大小样式
        /// </summary>
        /// <returns></returns>
        protected string RenderModalSize()
        {
            var ret = "";
            switch (Size)
            {
                case ModalSize.Default:
                    break;
                case ModalSize.Small:
                    ret = "modal-sm";
                    break;
                case ModalSize.Large:
                    ret = "modal-lg";
                    break;
                case ModalSize.ExtraLarge:
                    ret = "modal-xl";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Toggle()
        {
            JSRuntime.ToggleModal($"#{Id}");
        }
    }

    /// <summary>
    /// 弹窗大小
    /// </summary>
    public enum ModalSize
    {
        /// <summary>
        /// 
        /// </summary>
        Default,
        /// <summary>
        /// 
        /// </summary>
        Small,
        /// <summary>
        /// 
        /// </summary>
        Large,
        /// <summary>
        /// 
        /// </summary>
        ExtraLarge,
    }
}
