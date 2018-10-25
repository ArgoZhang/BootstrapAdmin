using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class InterfaceController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<BootstrapDict> RetrieveDicts()
        {
            return DictHelper.RetrieveDicts();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> RetrieveRolesByUrl([FromBody]string url)
        {
            return RoleHelper.RetrieveRolesByUrl(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> RetrieveRolesByUserName([FromBody]string userName)
        {
            return RoleHelper.RetrieveRolesByUserName(userName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BootstrapUser RetrieveUserByUserName([FromBody]string userName)
        {
            return UserHelper.RetrieveUserByUserName(userName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<BootstrapMenu> RetrieveAppMenus([FromBody]AppMenuOption args)
        {
            return MenuHelper.RetrieveAppMenus(args.AppId, args.UserName, args.Url);
        }
    }
}
