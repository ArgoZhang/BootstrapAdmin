using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskModel : NavigatorBarModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public TaskModel(ControllerBase controller) : base(controller)
        {
            Tasks = new string[] { "测试任务" };
        }

        /// <summary>
        /// 获得 系统配置的所有任务
        /// </summary>
        public IEnumerable<string> Tasks { get; }
    }
}
