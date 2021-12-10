using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 弹窗组件基类
    /// </summary>
    public class AlertBase : ModalBase
    {
        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public RenderFragment? AlertBody { get; set; }

        /// <summary>
        /// 获得/设置 弹窗 Footer 代码块
        /// </summary>
        [Parameter]
        public RenderFragment? AlertFooter { get; set; }

        /// <summary>
        /// 获得/设置 是否自动关闭 默认为 true
        /// </summary>
        [Parameter]
        public bool AutoClose { get; set; } = true;

        /// <summary>
        /// 获得/设置 自动关闭时长 默认 1500 毫秒
        /// </summary>
        [Parameter]
        public int Interval { get; set; } = 1500;

        /// <summary>
        /// 控件渲染完毕后回调方法
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (_show)
            {
                _show = false;
                Toggle();
            }
        }

        private bool _show;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        public void Show(string title)
        {
            Title = title;
            _show = true;
            StateHasChanged();
        }
    }
}
