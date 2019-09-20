using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 网站设置 Model 实体类
    /// </summary>
    public class SettingsModel : NavigatorBarModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public SettingsModel(ControllerBase controller) : base(controller)
        {
            Themes = DictHelper.RetrieveThemes();
        }

        /// <summary>
        /// 获得 系统配置的所有样式表
        /// </summary>
        public IEnumerable<BootstrapDict> Themes { get; }
    }
}
