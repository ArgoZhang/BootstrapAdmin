using Bootstrap.Admin.Extensions;
using Bootstrap.Admin.Shared;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public PageSet? PageSet { get; set; }

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
        /// 添加新 Tab 栏方法
        /// </summary>
        /// <param name="menu"></param>
        public void AddTab(BootstrapMenu? menu)
        {
            if (menu != null)
            {
                TabSet?.AddTab(menu);
                PageSet?.AddPage(menu);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabId"></param>
        public async Task CloseTab(string? tabId)
        {
            if (TabSet != null)
            {
                if (TabSet.TabCount == 1) RootLayout.NavigationManager?.NavigateTo(RootLayout.HomeUrl);
                else
                {
                    var pageId = await TabSet.CloseTab(tabId);
                    PageSet?.RemovePage(tabId, pageId);
                }
            }
        }

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
            AddTab(menu);
        }
    }
}
