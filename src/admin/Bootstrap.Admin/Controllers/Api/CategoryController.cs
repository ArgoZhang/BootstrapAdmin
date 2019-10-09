using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 数据字典分类
    /// </summary>
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        /// <summary>
        /// 获取字典表中所有Category数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<string> RetrieveDictCategorys()
        {
            return DictHelper.RetrieveCategories();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> RetrieveMenus()
        {
            return MenuHelper.RetrieveAllMenus(User.Identity.Name).OrderBy(m => m.Name).Select(m => m.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> RetrieveParentMenus()
        {
            return MenuHelper.RetrieveMenus(User.Identity.Name).Where(m => m.Menus.Count() > 0).OrderBy(m => m.Name).Select(m => m.Name);
        }
    }
}
