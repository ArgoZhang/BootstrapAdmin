using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Client.Models
{
    /// <summary>
    /// Mail Model
    /// </summary>
    public class MailModel : NavigatorBarModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MailModel(ControllerBase controller) : base(controller)
        {

        }

        /// <summary>
        /// 获得执行结果
        /// </summary>
        public string Result { get; set; } = "";
    }
}
