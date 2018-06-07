using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    [Route("api/[controller]")]
    public class CssController : Controller
    {
        /// <summary>
        /// 获得当前样式接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Get()
        {
            return new JsonResult(DictHelper.RetrieveActiveCss().FirstOrDefault().Code);
        }
        /// <summary>
        /// 获得网站所有样式表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<BootstrapDict> Post()
        {
            return DictHelper.RetrieveWebCss();
        }
    }
}
