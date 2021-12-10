namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 组件位置枚举类型
    /// </summary>
    public enum Placement
    {
        /// <summary>
        /// 
        /// </summary>
        Default,
        /// <summary>
        /// 
        /// </summary>
        Top,
        /// <summary>
        /// 
        /// </summary>
        TopLeft,
        /// <summary>
        /// 
        /// </summary>
        TopRight,
        /// <summary>
        /// 
        /// </summary>
        Bottom,
        /// <summary>
        /// 
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 
        /// </summary>
        BottomRight,
        /// <summary>
        /// 
        /// </summary>
        Center
    }

    /// <summary>
    /// 
    /// </summary>
    public static class PlacementExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="placement"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string ToCss(this Placement placement, string prefix = "")
        {
            string ret = "";
            switch (placement)
            {
                case Placement.Center:
                    ret = "center";
                    break;
                case Placement.Top:
                    ret = "top";
                    break;
                case Placement.TopLeft:
                    ret = "top-left";
                    break;
                case Placement.TopRight:
                    ret = "top-right";
                    break;
                case Placement.Bottom:
                    ret = "bottom";
                    break;
                case Placement.BottomLeft:
                    ret = "bottom-left";
                    break;
                case Placement.BottomRight:
                case Placement.Default:
                    ret = "bottom-right";
                    break;
            }
            return string.IsNullOrEmpty(prefix) ? ret : $"{prefix}-{ret}";
        }
    }
}
