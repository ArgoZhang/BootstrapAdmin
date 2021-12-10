using Bootstrap.Client.DataAccess;
using Bootstrap.Security.Mvc;

namespace Bootstrap.Client.Models
{
    /// <summary>
    /// ModelBase 基础类
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ModelBase()
        {
            AppId = BootstrapAppContext.AppId;
            Title = DictHelper.RetrieveWebTitle(AppId);
            Footer = DictHelper.RetrieveWebFooter(AppId);
            Theme = DictHelper.RetrieveActiveTheme();
            IsDemo = DictHelper.RetrieveSystemModel();
        }

        /// <summary>
        /// 是否为演示系统
        /// </summary>
        public bool IsDemo { get; protected set; }

        /// <summary>
        /// 获得 应用程序标识
        /// </summary>
        public string AppId { get; private set; }

        /// <summary>
        /// 获取 网站标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 获取 网站页脚
        /// </summary>
        public string Footer { get; private set; }

        /// <summary>
        /// 网站样式全局设置
        /// </summary>
        public string Theme { get; protected set; }

        /// <summary>
        /// 是否显示卡片标题
        /// </summary>
        public string ShowCardTitle { get; protected set; } = "";

        /// <summary>
        /// 是否收缩侧边栏
        /// </summary>
        public string ShowSideBar { get; protected set; } = "";
    }
}
