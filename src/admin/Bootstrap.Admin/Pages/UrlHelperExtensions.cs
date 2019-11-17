namespace Bootstrap.Admin.Pages
{
    /// <summary>
    /// Url 地址辅助操作类
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// 转换为 Blazor 地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToBlazorLink(this string url) => url.TrimStart('~');

        /// <summary>
        /// 转化为 Blazor 菜单地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToBlazorMenuUrl(this string url) => url.Replace("~", "/Pages");
    }
}
