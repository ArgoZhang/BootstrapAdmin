namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginModel
    {
        public LoginModel()
        {
            UserName = "Argo";
            Password = "1111";
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
    }
}