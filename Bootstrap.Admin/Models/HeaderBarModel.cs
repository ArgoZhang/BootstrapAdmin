using Bootstrap.DataAccess;
using Bootstrap.Security;
using System.Security.Principal;
using System.Web;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderBarModel : ModelBase
    {
        public HeaderBarModel(IIdentity identity)
        {
            var user = BootstrapUser.RetrieveUserByUserName(identity.Name);
            Icon = user.Icon;
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            Css = user.Css;
        }
        public string UserName { get; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; }
        /// <summary>
        /// 
        /// </summary>
        public string HomeUrl { get; set; }
        /// <summary>
        /// 获得/设置 用户头像地址
        /// </summary>
        public string Icon { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Css { get; }
    }
}