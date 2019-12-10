using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderBase : BootstrapComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string UserName { get; set; } = "未设置";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<string>? DisplayNameChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Icon { get; set; } = "";

        /// <summary>
        /// 更新登录用户显示名称方法
        /// </summary>
        /// <param name="displayName"></param>
        public void UpdateDisplayName(string displayName)
        {
            StateHasChanged();
        }
    }
}
