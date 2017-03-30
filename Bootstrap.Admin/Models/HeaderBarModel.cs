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
            Icon = user.Icon;
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            UserId = user.Id;
        }
        public string UserName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string HomeUrl { get; set; }
        /// <summary>
        /// 获得/设置 用户头像地址
        /// </summary>
        public string Icon { get; private set; }
    }
}