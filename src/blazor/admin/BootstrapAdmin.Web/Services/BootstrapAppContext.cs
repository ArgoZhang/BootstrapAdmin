using Microsoft.AspNetCore.Components.Authorization;

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
        [NotNull]
        public string? UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public string? DisplayName { get; internal set; }

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
