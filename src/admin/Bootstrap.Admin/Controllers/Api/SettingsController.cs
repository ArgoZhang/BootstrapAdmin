using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 网站设置控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        /// <summary>
        /// 保存网站设置方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Settings", Auth = "saveTitle,saveFooter,saveTheme,saveUISettings,clearCache,clearAllCache,loginSettings,lockScreen,defaultApp,blazor")]
        public bool Post([FromBody]BootstrapDict value) => DictHelper.SaveSettings(value);

        /// <summary>
        /// 获取网站缓存站点集合
        /// </summary>
        [HttpGet]
        public IEnumerable<ICacheCorsItem> Get() => CacheManager.CorsSites;
    }
}
