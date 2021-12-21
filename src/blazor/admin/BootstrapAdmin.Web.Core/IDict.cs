using BootstrapBlazor.Components;

namespace BootstrapAdmin.Web.Core
{
    /// <summary>
    /// Dict 字典表接口
    /// </summary>
    public interface IDict
    {
        List<SelectedItem> GetApps();

        /// <summary>
        /// 获取 站点 Title 配置信息
        /// </summary>
        /// <returns></returns>
        string GetWebTitle();

        /// <summary>
        /// 获取站点 Footer 配置信息
        /// </summary>
        /// <returns></returns>
        string GetWebFooter();
    }
}
