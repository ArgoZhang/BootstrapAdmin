namespace BootstrapAdmin.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapAppContext
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public BootstrapAppContext(IConfiguration configuration)
        {
            AppId = configuration.GetValue("AppId", "BA");
        }
    }
}
