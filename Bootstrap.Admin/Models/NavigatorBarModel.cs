using Bootstrap.DataAccess;
using Bootstrap.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class NavigatorBarModel : HeaderBarModel
    {
        public NavigatorBarModel(string url)
        {
            Navigations = BootstrapMenu.RetrieveSystemMenus(UserName);
            Applications = DictHelper.RetrieveApps();
            ActiveMenu(null, Navigations.ToList(), url);
            HomeUrl = "~/Admin/Index";
        }

        private void ActiveMenu(BootstrapMenu parent, List<BootstrapMenu> menus, string url)
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
        public IEnumerable<BootstrapMenu> Navigations { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Applications { get; private set; }
    }
}