using Bootstrap.DataAccess;
using Bootstrap.Security;
using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    public class NavigatorBarModel : HeaderBarModel
    {
        public NavigatorBarModel(string url)
        {
            Navigations = BootstrapMenu.RetrieveSystemMenus(UserName, url);
            Applications = DictHelper.RetrieveApps();
            HomeUrl = "~/Admin/Index";
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