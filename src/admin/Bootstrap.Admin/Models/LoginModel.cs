using Bootstrap.DataAccess;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginModel : ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public LoginModel()
        {
            ImageLibUrl = DictHelper.RetrieveImagesLibUrl();
        }

        /// <summary>
        /// 验证码图床地址
        /// </summary>
        public string ImageLibUrl { get; protected set; }

        /// <summary>
        /// 是否登录认证失败
        /// </summary>
        public bool AuthFailed { get; set; }
    }
}
