using BootstrapBlazor.Components;

namespace Bootstrap.Admin.Blazor.Shared
{
    /// <summary>
    /// AdminLayout 布局类
    /// </summary>
    public partial class AdminLayout
    {
        private IEnumerable<MenuItem>? MenuItems { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            MenuItems = DataAccess.MenuHelper.RetrieveSystemMenus("Admin", "admin/dicts").Select(s => new MenuItem() { Url = s.Url.Replace("~", "").ToLower(), Text = s.Name, Icon = s.Icon, Items = s.Menus.Select(x => new MenuItem { Url = x.Url.Replace("~", "").ToLower(), Text = x.Name, Icon = x.Icon }) });
        }
    }
}
