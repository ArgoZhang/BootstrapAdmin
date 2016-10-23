using Bootstrap.DataAccess;
using System.Web;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderBarModel : ModelBase
    {
        public HeaderBarModel()
        {
            DisplayName = UserHelper.RetrieveUsersByName(HttpContext.Current.User.Identity.Name).DisplayName;
            HomeUrl = "~/";
        }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BreadcrumbName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShowMenu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HomeUrl { get; set; }
    }
}