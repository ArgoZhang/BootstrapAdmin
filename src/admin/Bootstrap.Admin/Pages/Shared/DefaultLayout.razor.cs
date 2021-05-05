using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace Bootstrap.Admin.Pages.Shared
{
    /// <summary>
    /// 默认模板页
    /// </summary>
    public partial class DefaultLayout
    {
        [NotNull]
        private NavigatorBarModel? Model { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string WebTitle { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string WebFooter { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获得/设置 系统首页
        /// </summary>
        public string HomeUrl { get; protected set; } = "Pages";

        /// <summary>
        /// 获得/设置 当前请求路径
        /// </summary>
        protected string RequestUrl { get; set; } = "";

        [NotNull]
        private IEnumerable<MenuItem>? Menus { get; set; }

        [Inject]
        [NotNull]
        private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (state != null)
            {
                IsAdmin = state.User.IsInRole("Administrators");
                UserName = state.User.Identity?.Name ?? "";
            }

            Menus = MenuHelper.RetrieveMenusByUserName(UserName).Select(m => new MenuItem()
            {
                Text = m.Name,
                Url = m.Url,
                Icon = m.Icon
            });
        }
    }
}
