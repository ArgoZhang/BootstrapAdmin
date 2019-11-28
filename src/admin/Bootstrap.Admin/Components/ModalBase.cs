using Bootstrap.Admin.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        public string Id { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "未设置";

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public RenderFragment? ModalBody { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool Backdrop { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public RenderFragment? ModalFooter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                JSRuntime.InitModal();
            }
        }
    }
}
