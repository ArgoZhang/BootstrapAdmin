using Bootstrap.Admin.Extensions;
using Bootstrap.Admin.Models;
using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
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
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = new ServerAuthenticationStateProvider();

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
        public NavigatorBarModel Model { get; set; } = new NavigatorBarModel("");

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
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获得/设置 系统首页
        /// </summary>
        public string HomeUrl { get; protected set; } = "/Pages";

        /// <summary>
        /// 获得/设置 当前请求路径
        /// </summary>
        protected string RequestUrl { get; set; } = "";

        /// <summary>
        /// Header 组件引用实例
        /// </summary>
        protected Header? Header { get; set; }

        /// <summary>
        /// SideBar 组件引用实例
        /// </summary>
        protected SideBar? SideBar { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!state.User.Identity.IsAuthenticated)
            {
                NavigationManager?.NavigateTo("/Account/Login?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery));
            }
            else
            {
                IsAdmin = state.User.IsInRole("Administrators");
                UserName = state.User.Identity.Name ?? "";
            }
        }

        /// <summary>
        /// 设置参数方法
        /// </summary>
        protected override void OnParametersSet()
        {
            RequestUrl = new UriBuilder(NavigationManager?.Uri ?? "").Path;
            Model = new NavigatorBarModel(UserName, RequestUrl.ToMvcMenuUrl());
            DisplayName = Model.DisplayName;
        }

        /// <summary>
        ///
        /// </summary>
        public void OnDisplayNameChanged(string displayName)
        {
            DisplayName = displayName;
            Header?.DisplayNameChanged.InvokeAsync(DisplayName);
            SideBar?.DisplayNameChanged.InvokeAsync(DisplayName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender) JSRuntime.InitDocument();
        }
    }
}
