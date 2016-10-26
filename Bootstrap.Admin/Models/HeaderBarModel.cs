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
            var user = UserHelper.RetrieveUsersByName(HttpContext.Current.User.Identity.Name);
            DisplayName = user.DisplayName;
            UserID = user.ID;
            HomeUrl = "~/";
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }
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