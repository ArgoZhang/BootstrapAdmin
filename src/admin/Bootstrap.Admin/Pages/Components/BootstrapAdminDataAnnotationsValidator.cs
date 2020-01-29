using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// BootstrapAdminDataAnnotationsValidator 验证组件
    /// </summary>
    public class BootstrapAdminDataAnnotationsValidator : ComponentBase
    {
        /// <summary>
        /// 获得/设置 当前编辑数据上下文
        /// </summary>
        [CascadingParameter]
        EditContext? CurrentEditContext { get; set; }

        /// <summary>
        /// 获得/设置 当前编辑窗体上下文
        /// </summary>
        [CascadingParameter]
        public LgbEditFormBase? EditForm { get; set; }

        /// <summary>
        /// 初始化方法
        /// </summary>
        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading parameter of type {nameof(EditContext)}. For example, you can use {nameof(DataAnnotationsValidator)} inside an EditForm.");
            }

            if (EditForm == null)
            {
                throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading parameter of type {nameof(LgbEditFormBase)}. For example, you can use {nameof(BootstrapAdminDataAnnotationsValidator)} inside an EditForm.");
            }

            CurrentEditContext.AddBootstrapAdminDataAnnotationsValidation(EditForm);
        }
    }
}
