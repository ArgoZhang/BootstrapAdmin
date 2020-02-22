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
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Settings", Auth = "saveTitle,saveFooter,saveTheme,saveUISettings,loginSettings,lockScreen,defaultApp,blazor,iplocate,logSettings")]
        public bool Post([FromBody]IEnumerable<BootstrapDict> values) => DictHelper.SaveUISettings(values);

        /// <summary>
        /// 保存网站是否为演示系统时调用
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(string id, [FromBody]BootstrapDict dict) => DictHelper.UpdateSystemModel(dict.Code == "1", dict.Name);

        /// <summary>
        /// 获取网站缓存站点集合
        /// </summary>
        [HttpGet]
        public IEnumerable<ICacheCorsItem> Get() => CacheManager.CorsSites;
    }
}
