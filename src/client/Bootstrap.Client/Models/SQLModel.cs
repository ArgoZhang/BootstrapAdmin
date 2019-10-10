using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Client.Models
{
    /// <summary>
    /// SQL Model
    /// </summary>
    public class SQLModel : NavigatorBarModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLModel(ControllerBase controller) : base(controller)
        {

        }

        /// <summary>
        /// 获得执行结果
        /// </summary>
        public int Result { get; set; }
    }
}
