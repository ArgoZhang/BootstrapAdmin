namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// Model 基类
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// 获取 网站 logo 小图标
        /// </summary>
        public string WebSiteIcon { get; protected set; } = "~/favicon.ico";

        /// <summary>
        /// 获得 网站图标
        /// </summary>
        public string WebSiteLogo { get; protected set; } = "~/favicon.png";
    }
}
