using System.IO;
using System.Web;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class InfosModel : NavigatorBarModel
    {
        /// <summary>
        /// 获得/设置 头像文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public InfosModel(string url) : base(url)
        {
            var fileName = HttpContext.Current.Server.MapPath(Icon);
            if (File.Exists(fileName))
            {
                Size = new FileInfo(fileName).Length;
            }
        }
    }
}