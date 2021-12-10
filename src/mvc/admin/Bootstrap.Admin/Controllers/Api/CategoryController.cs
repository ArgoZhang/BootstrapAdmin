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
        /// 获取字典表中所有 Category 数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<string> RetrieveDictCategorys()
        {
            return DictHelper.RetrieveCategories();
        }

        /// <summary>
        /// 获取所有菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> RetrieveMenus()
        {
            return MenuHelper.RetrieveAllMenus(User.Identity!.Name).OrderBy(m => m.Name).Select(m => m.Name);
        }

        /// <summary>
        /// 获取所有父级菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> RetrieveParentMenus()
        {
            return MenuHelper.RetrieveMenus(User.Identity!.Name).Where(m => m.Menus.Count() > 0).OrderBy(m => m.Name).Select(m => m.Name);
        }

        /// <summary>
        /// 通过指定菜单检查子菜单是否有子菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public bool ValidateMenuBySubMenu(string id)
        {
            return !MenuHelper.RetrieveAllMenus(User.Identity!.Name).Where(m => m.ParentId == id).Any();
        }

        /// <summary>
        /// 通过指定菜单检查父级菜单是否为菜单类型 资源与按钮返回 false
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public bool ValidateParentMenuById(string id)
        {
            return MenuHelper.RetrieveAllMenus(User.Identity!.Name).FirstOrDefault(m => m.Id == id)?.IsResource == 0;
        }
    }
}
