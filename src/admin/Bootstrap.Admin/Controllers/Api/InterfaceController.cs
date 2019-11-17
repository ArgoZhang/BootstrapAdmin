using Bootstrap.DataAccess;
using Bootstrap.Security;
using Bootstrap.Security.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 接口控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    [ApiController]
    public class InterfaceController : ControllerBase
    {
        /// <summary>
        /// 获取所有字典表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<BootstrapDict> RetrieveDicts()
        {
            return DictHelper.RetrieveDicts();
        }

        /// <summary>
        /// 通过请求地址获取相对应角色集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> RetrieveRolesByUrl([FromBody]string url)
        {
            return RoleHelper.RetrievesByUrl(url, BootstrapAppContext.AppId);
        }

        /// <summary>
        /// 通过用户名获得分配所有角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> RetrieveRolesByUserName([FromBody]string userName)
        {
            return RoleHelper.RetrievesByUserName(userName);
        }

        /// <summary>
        /// 通过用户名获得 User 实例
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BootstrapUser? RetrieveUserByUserName([FromBody]string userName)
        {
            return UserHelper.RetrieveUserByUserName(userName);
        }

        /// <summary>
        /// 通过指定条件获得应用程序菜单
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<BootstrapMenu> RetrieveAppMenus([FromBody]AppMenuOption args)
        {
            if (string.IsNullOrEmpty(args.AppId) || string.IsNullOrEmpty(args.UserName)) return new BootstrapMenu[0];
            return MenuHelper.RetrieveAppMenus(args.AppId, args.UserName, args.Url);
        }

        /// <summary>
        /// 发送健康检查结果
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Healths([FromServices]GiteeHttpClient httpClient, [FromServices]IConfiguration config, [FromBody]string message)
        {
            var ret = false;
            var url = config.GetValue("HealthsCloudUrl", "");
            if (!string.IsNullOrEmpty(url))
            {
                try { await httpClient.HttpClient.PostAsJsonAsync(url, message); ret = true; }
                catch { }
            }
            return ret;
        }
    }
}
