using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Active { get; set; }

        public static List<Menu> RetrieveMenus()
        {
            return new List<Menu>() {
                new Menu() { Name = "菜单管理", Icon = "fa-dashboard", Url="javascript:;", Active = "" },
                new Menu() { Name = "用户管理", Icon = "fa-user", Url="/Admin/Users", Active = "active" },
                new Menu() { Name = "角色管理", Icon = "fa-sitemap", Url="javascript:;", Active = "" },
                new Menu() { Name = "部门管理", Icon = "fa-home", Url="javascript:;", Active = "" },
                new Menu() { Name = "字典表维护", Icon = "fa-book", Url="javascript:;", Active = "" },
                new Menu() { Name = "个性化维护", Icon = "fa-pencil", Url="javascript:;", Active = "" },
                new Menu() { Name = "系统日志", Icon = "fa-lock", Url="javascript:;", Active = "" }
            };
        }
    }
}
