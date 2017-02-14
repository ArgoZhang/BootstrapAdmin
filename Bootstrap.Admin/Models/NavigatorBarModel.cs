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
            Navigations = MenuHelper.RetrieveNavigationsByUserName(UserName).Where(m => m.IsResource == 0);
            ActiveMenu(null, Navigations.ToList(), url);
            HomeUrl = "~/Admin/Index";
        }

        private void ActiveMenu(Menu parent, List<Menu> menus, string url)
        {
            menus.ForEach(m =>
            {
                m.Active = m.Url.Equals(url, StringComparison.OrdinalIgnoreCase) ? "active" : "";
                ActiveMenu(m, m.Menus.ToList(), url);
                if (parent != null && m.Active != "") parent.Active = m.Active;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Menu> Navigations { get; set; }
    }
}