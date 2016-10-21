namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderBarModel : ModelBase
    {
        public HeaderBarModel()
        {
            UserName = "Argo Zhang";
            HomeUrl = "~/";
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BreadcrumbName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShowMenu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HomeUrl { get; set; }
    }
}