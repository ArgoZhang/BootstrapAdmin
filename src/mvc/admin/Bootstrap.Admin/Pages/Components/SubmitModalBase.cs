using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 数据绑定提交弹窗组件
    /// </summary>
    public class SubmitModalBase<TItem> : ModalBase
    {
#nullable disable
        /// <summary>
        /// 获得/设置 弹窗绑定数据实体
        /// </summary>
        [Parameter]
        public TItem Model { get; set; }
#nullable restore

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<TItem> ModelChanged { get; set; }

        /// <summary>
        /// A callback that will be invoked when the form is submitted.
        /// If using this parameter, you are responsible for triggering any validation
        /// manually, e.g., by calling <see cref="EditContext.Validate"/>.
        /// </summary>
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }

        /// <summary>
        /// A callback that will be invoked when the form is submitted and the
        /// <see cref="EditContext"/> is determined to be valid.
        /// </summary>
        [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }

        /// <summary>
        /// A callback that will be invoked when the form is submitted and the
        /// <see cref="EditContext"/> is determined to be invalid.
        /// </summary>
        [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }
    }
}
