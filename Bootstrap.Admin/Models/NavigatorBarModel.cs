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
            Navigations = MenuHelper.RetrieveNavigationsByUserName(UserName);
            Navigations.ToList().ForEach(m => m.Active = m.Url.Equals(url, StringComparison.OrdinalIgnoreCase) ? "active" : "");
            HomeUrl = "~/Admin/Index";
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Menu> Navigations { get; set; }
    }
}