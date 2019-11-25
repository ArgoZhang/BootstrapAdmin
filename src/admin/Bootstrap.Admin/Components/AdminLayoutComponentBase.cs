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
        public DefaultLayout Layout { get; protected set; } = new DefaultLayout();

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
                if (TabSet.TabCount == 1) Layout.NavigationManager?.NavigateTo(Layout.HomeUrl);
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
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) Layout.JSRuntime.EnableAnimation();

            var requestUrl = Layout.NavigationManager?.Uri ?? "";
            var path = new Uri(requestUrl).PathAndQuery;
            var menus = DataAccess.MenuHelper.RetrieveAllMenus(Layout.UserName);
            var menu = menus.FirstOrDefault(menu => path.Contains(menu.Url.ToBlazorMenuUrl()));
            AddTab(menu);
            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
