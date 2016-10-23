namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LockModel : LoginModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}