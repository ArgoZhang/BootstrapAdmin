using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfilesModel : ThemeModel
    {
        /// <summary>
        /// 获得/设置 头像文件大小
        /// </summary>
        public long Size { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public ProfilesModel(ControllerBase controller) : base(controller)
        {
            var host = controller.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            if (host == null) return;
            var fileName = Path.Combine(host.WebRootPath, Icon.TrimStart('~', '/').Replace('/', '\\'));
            if (File.Exists(fileName))
            {
                Size = new FileInfo(fileName).Length;
            }
        }
    }
}