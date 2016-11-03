using Bootstrap.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class NavigatorBarModel : HeaderBarModel
    {
        public NavigatorBarModel(string url)
        {
            Menus = MenuHelper.RetrieveMenus().ToList();
            Menus.ForEach(m => m.Active = m.Url.Equals(url, StringComparison.OrdinalIgnoreCase) ? "active" : "");
            HomeUrl = "~/Admin/Index";
            ShowMenu = "hide";
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Menu> Menus { get; set; }
    }
}