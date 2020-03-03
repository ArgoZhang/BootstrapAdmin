using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 模态框组件类
    /// </summary>
    public class ModalBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 IJSRuntime 实例
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 获得/设置 ModalBody 代码块
        /// </summary>
        [Parameter]
        public RenderFragment? ModalBody { get; set; }

        /// <summary>
        /// 获得/设置 弹窗 Footer 代码块
        /// </summary>
        [Parameter]
        public RenderFragment? ModalFooter { get; set; }

        /// <summary>
        /// 获得/设置 Id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 获得/设置 弹窗标题
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
        /// SetParametersAsync 方法
        /// </summary>
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (string.IsNullOrEmpty(Id)) throw new InvalidOperationException("Modal Component Id property must be set");
            return base.SetParametersAsync(ParameterView.Empty);
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
        /// Toggle 弹窗方法
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
        /// 默认大小
        /// </summary>
        Default,
        /// <summary>
        /// 小窗口
        /// </summary>
        Small,
        /// <summary>
        /// 大窗口
        /// </summary>
        Large,
        /// <summary>
        /// 超大窗口
        /// </summary>
        ExtraLarge,
    }
}
