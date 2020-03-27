namespace Bootstrap.Client.Extensions
{
    /// <summary>
    /// SmtpMessage 邮件实体类
    /// </summary>
    internal class SmtpMessage
    {
        /// <summary>
        /// 获得/设置 邮件标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 获得/设置 邮件正文
        /// </summary>
        public string Message { get; set; } = "";
    }
}
