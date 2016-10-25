using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 获得/设置 菜单主键ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 获得/设置 父级菜单ID
        /// </summary>
        public int ParentId { set; get; }
        /// <summary>
        /// 获得/设置 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获得/设置 菜单序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 获得/设置 菜单图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 获得/设置 菜单URL地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 获得/设置 菜单分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Active { get; set; }

        public static List<Menu> RetrieveMenus()
        {
            return new List<Menu>() {
                new Menu() { Name = "菜单管理", Icon = "fa-dashboard", Url="javascript:;", Active = "" },
                new Menu() { Name = "用户管理", Icon = "fa-user", Url="/Admin/Users", Active = "" },
                new Menu() { Name = "角色管理", Icon = "fa-sitemap", Url="/Admin/Roles", Active = "" },
                new Menu() { Name = "部门管理", Icon = "fa-home", Url="/Admin/Groups", Active = "" },
                new Menu() { Name = "字典表维护", Icon = "fa-book", Url="javascript:;", Active = "" },
                new Menu() { Name = "个性化维护", Icon = "fa-pencil", Url="javascript:;", Active = "" },
                new Menu() { Name = "系统日志", Icon = "fa-lock", Url="javascript:;", Active = "" }
            };
        }
    }
}
