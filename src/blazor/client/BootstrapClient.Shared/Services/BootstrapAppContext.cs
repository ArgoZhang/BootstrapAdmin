using Microsoft.Extensions.Configuration;

namespace BootstrapClient.Web.Shared.Services
{
    /// <summary>
    /// AppContext 实体类
    /// </summary>
    public class BootstrapAppContext
    {
        /// <summary>
        /// 获得/设置 当前网站 AppId
        /// </summary>
        public string AppId { get; }

        /// <summary>
        /// 获得/设置 当前登录账号
        /// </summary>
        [NotNull]
        public string? UserName { get; set; }

        /// <summary>
        /// 获得/设置 当前用户显示名称
        /// </summary>
        [NotNull]
        public string? DisplayName { get; internal set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public BootstrapAppContext(IConfiguration configuration)
        {
            AppId = configuration.GetValue("AppId", "BA");
        }
    }
}
