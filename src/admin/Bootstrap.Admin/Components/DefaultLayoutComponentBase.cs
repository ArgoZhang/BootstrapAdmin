using Bootstrap.Admin.Models;
using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultLayoutComponentBase : LayoutComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NavigatorBarModel? Model { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SideBar? SideBar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Header? Header { get; set; }

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
        protected bool IsAdmin { get; set; }

        /// <summary>
        /// 获得/设置 系统首页
        /// </summary>
        public string HomeUrl { get; protected set; } = "/Pages";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationStateProvider != null)
            {
                var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (!state.User.Identity.IsAuthenticated)
                {
                    NavigationManager?.NavigateTo("/Account/Login?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery));
                }
                else
                {
                    Model = new NavigatorBarModel(state.User.Identity.Name);
                    IsAdmin = state.User.IsInRole("Administrators");
                    UserName = state.User.Identity.Name ?? "";
                    DisplayName = Model.DisplayName;
                }
            }
        }
    }
}
