namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginModel : ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remember { get; set; }
    }
}