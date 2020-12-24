using Bootstrap.Admin.Models;
using Bootstrap.Admin.Pages.Components;
using Bootstrap.Admin.Pages.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Pages.Shared
{
    /// <summary>
    ///
    /// </summary>
    public partial class DefaultLayout
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
        /// 获得/设置 组件名字
        /// </summary>
        [Inject]
        protected IHttpContextAccessor? HttpContextAccessor { get; set; }

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
        public string WebTitle { get; set; } = "";

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

        /// <summary>
        /// Header 组件引用实例
        /// </summary>
        protected Header? Header { get; set; }

        /// <summary>
        /// SideBar 组件引用实例
        /// </summary>
        protected SideBar? SideBar { get; set; }

        /// <summary>
        /// Footer 组件引用实例
        /// </summary>
        protected Footer? Footer { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            // 网页跳转监控
            if (NavigationManager != null)
            {
                NavigationManager.LocationChanged += NavigationManager_LocationChanged;
            }

            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!state.User.Identity!.IsAuthenticated)
            {
                NavigationManager?.NavigateTo("/Account/Login?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery));
            }
            else
            {
                IsAdmin = state.User.IsInRole("Administrators");
                UserName = state.User.Identity.Name ?? "";
            }
        }

        private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
        {
            var name = $"/{NavigationManager?.ToBaseRelativePath(e.Location)}";
            if (HttpContextAccessor != null) HttpContextAccessor.HttpContext?.SaveOnlineUser(name);
        }

        /// <summary>
        /// OnParametersSetAsync 方法
        /// </summary>
        protected override void OnParametersSet()
        {
            if (NavigationManager != null)
            {
                RequestUrl = new UriBuilder(NavigationManager.Uri).Path;
                Model = new NavigatorBarModel(UserName, RequestUrl.ToMvcMenuUrl());
                DisplayName = Model.DisplayName;
                WebTitle = Model.Title;
                WebFooter = Model.Footer;
            }
        }

        /// <summary>
        /// 显示名称变化时方法
        /// </summary>
        public void OnDisplayNameChanged(string displayName)
        {
            DisplayName = displayName;
            Header?.DisplayNameChanged.InvokeAsync(DisplayName);
            SideBar?.DisplayNameChanged.InvokeAsync(DisplayName);
        }

        /// <summary>
        /// 网站标题变化时触发此方法
        /// </summary>
        /// <param name="title"></param>
        public void OnWebTitleChanged(string title)
        {
            Header?.WebTitleChanged.InvokeAsync(title);
            SideBar?.WebTitleChanged.InvokeAsync(title);
        }

        /// <summary>
        /// 获得/设置 网站页脚文字
        /// </summary>
        /// <value></value>
        public string WebFooter { get; set; } = "";

        /// <summary>
        /// 网站页脚文字变化是触发此方法
        /// </summary>
        /// <param name="text"></param>
        public void OnWebFooterChanged(string text) => Footer?.TextChanged.InvokeAsync(text);

        /// <summary>
        /// OnAfterRender 方法
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender) JSRuntime.InitDocument();
        }
    }
}
