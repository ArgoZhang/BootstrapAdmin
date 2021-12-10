using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Client.Controllers.Api
{
    /// <summary>
    /// 运维管理接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// 更改系统运行模式 1 为演示模式 0 为正常模式
        /// </summary>
        [HttpGet]
        public bool Get([FromQuery]string authCode, string salt, int model)
        {
            var ret = false;
            // 检查授权
            if (Longbow.Security.Cryptography.LgbCryptography.ComputeHash(authCode, salt) == "3lpCnRu7qqiAbIrx7gNRUB0mPXgJC3wGoyPJED3KeoA=")
            {
                using (var db = Longbow.Data.DbManager.Create("ba"))
                {
                    db.Execute("Update Dicts Set Code = @0 Where Category = @1 and Name = @2", model, "网站设置", "演示系统");
                }
                ret = true;
            }
            return ret;
        }
    }
}
