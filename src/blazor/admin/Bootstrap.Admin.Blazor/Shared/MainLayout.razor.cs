namespace Bootstrap.Admin.Blazor.Shared
{
    /// <summary>
    /// MainLayout 布局类
    /// </summary>
    public partial class MainLayout
    {
        private IEnumerable<MenuItem>? MenuItems { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            // TODO: 暂时写死 Admin 账号
            //MenuItems = DataAccess.MenuHelper.RetrieveSystemMenus("Admin").Select(s => new MenuItem()
            //{
            //    Url = s.Url.Replace("~", ""),
            //    Text = s.Name,
            //    Icon = s.Icon,
            //    Items = s.Menus.Select(x => new MenuItem
            //    {
            //        Url = x.Url.Replace("~", ""),
            //        Text = x.Name,
            //        Icon = x.Icon
            //    })
            //});
        }
    }
}
