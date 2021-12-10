using Longbow.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Client.Controllers.Api
{
    /// <summary>
    /// 运维邮件发送接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EncrptyController : ControllerBase
    {
        /// <summary>
        /// 生成加密盐值方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Salt()
        {
            return new JsonResult(LgbCryptography.GenerateSalt());
        }

        /// <summary>
        /// 根据提供的原始密码与盐值计算 Hash 值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Hash([FromBody]EncrptyPostData data)
        {
            return new JsonResult(LgbCryptography.ComputeHash(data.Password, data.Salt));
        }
    }

    /// <summary>
    /// 加密数据提交类
    /// </summary>
    public class EncrptyPostData
    {
        /// <summary>
        /// 获得/设置 加密盐值
        /// </summary>
        public string Salt { get; set; } = "";

        /// <summary>
        /// 获得/设置 要加密的原始密码
        /// </summary>
        public string Password { get; set; } = "";
    }
}
