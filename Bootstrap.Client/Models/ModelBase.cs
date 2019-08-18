using Bootstrap.Security.DataAccess;

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
            Title = DbHelper.RetrieveTitle();
            Footer = DbHelper.RetrieveFooter();
            Theme = DbHelper.RetrieveActiveTheme();
        }

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
