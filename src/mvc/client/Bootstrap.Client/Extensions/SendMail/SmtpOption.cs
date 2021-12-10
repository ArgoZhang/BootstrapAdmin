using System.Collections.Generic;

namespace Bootstrap.Client.Extensions
{
    /// <summary>
    /// SmtpOption 配置类
    /// </summary>
    public class SmtpOption
    {
        /// <summary>
        /// 获得/设置 主机地址
        /// </summary>
        public string Host { get; set; } = "";

        /// <summary>
        /// 获得/设置 邮箱密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// 获得/设置 发件人地址
        /// </summary>
        public string From { get; set; } = "";

        /// <summary>
        /// 获得/设置 收件人地址
        /// </summary>
        public string To { get; set; } = "";

        /// <summary>
        /// 获得/设置 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获得/设置 是否启用 SSL
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// 获得/设置 邮件发送人显示名称
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// 获得/设置 邮件发送超时时间 默认 100000 毫秒
        /// </summary>
        public int Timeout { get; set; } = 100000;

        /// <summary>
        /// 获得/设置 邮件黑名单
        /// </summary>
        public ICollection<string> BlackList { get; set; } = new HashSet<string>();
    }
}
