namespace Bootstrap.Client.Extensions
{
    /// <summary>
    /// MessageFormat 扩展方法
    /// </summary>
    internal static class MessageFormatExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToTitle(this MessageFormat format) => format switch
        {
            MessageFormat.Exception => "BootstrapAdmin Exception",
            MessageFormat.Healths => "Healths Report",
            MessageFormat.Test => "Send Mail Test",
            _ => ""
        };
    }
}
