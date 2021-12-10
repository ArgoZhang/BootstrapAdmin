namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    public enum Color
    {
        /// <summary>
        /// 
        /// </summary>
        Primary,
        /// <summary>
        /// 
        /// </summary>
        Secondary,
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 
        /// </summary>
        Danger,
        /// <summary>
        /// 
        /// </summary>
        Warning,
        /// <summary>
        /// 
        /// </summary>
        Info,
        /// <summary>
        /// 
        /// </summary>
        Light,
        /// <summary>
        /// 
        /// </summary>
        Dark,
        /// <summary>
        /// 
        /// </summary>
        White,
        /// <summary>
        /// 
        /// </summary>
        Transparent
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string ToCss(this Color color, string prefix = "")
        {
            string ret = color.ToString().ToLowerInvariant();
            return string.IsNullOrEmpty(prefix) ? ret : $"{prefix}-{ret}";
        }
    }
}
