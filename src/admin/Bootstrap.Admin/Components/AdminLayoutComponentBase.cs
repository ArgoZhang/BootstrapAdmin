using Bootstrap.Admin.Extensions;
using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// AdminLayout 布局类
    /// </summary>
    public class AdminLayoutComponentBase : LayoutComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public TabSet? TabSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout RootLayout { get; protected set; } = new DefaultLayout();

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
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender) RootLayout.JSRuntime.EnableAnimation();

            var requestUrl = RootLayout.NavigationManager?.Uri ?? "";
            var path = new Uri(requestUrl).PathAndQuery;
            var menus = DataAccess.MenuHelper.RetrieveAllMenus(RootLayout.UserName);
            var menu = menus.FirstOrDefault(menu => path.Contains(menu.Url.ToBlazorMenuUrl()));
            if (menu != null) TabSet?.AddTab(menu);
        }
    }
}
