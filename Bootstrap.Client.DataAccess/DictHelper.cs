using Bootstrap.Security.DataAccess;
using System.Linq;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictHelper
    {
        /// <summary>
        /// 获取验证码图床
        /// </summary>
        /// <returns></returns>
        public static string RetrieveImagesLibUrl() => DbHelper.RetrieveDictsWithCache().FirstOrDefault(d => d.Name == "验证码图床" && d.Category == "系统设置" && d.Define == 0)?.Code ?? "http://images.sdgxgz.com/";
    }
}
