using Longbow.Web.SMS;
using System.Threading.Tasks;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 手机号登陆帮助类
    /// </summary>
    class DefaultSMSProvider : ISMSProvider
    {
        public DefaultSMSProvider()
        {
            Options = new SMSOptions();
            Options.Roles.Add("Administrators");
            Options.Roles.Add("Default");
        }

        /// <summary>
        /// 获得 短信配置信息
        /// </summary>
        public SMSOptions Options { get; protected set; }

        /// <summary>
        /// 下发验证码方法
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task<SMSResult> SendCodeAsync(string phoneNumber) => Task.FromResult(new SMSResult() { Result = true });

        /// <summary>
        /// 验证验证码方法
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public bool Validate(string phone, string code) => code == "1234";
    }
}
