using System;

namespace Bootstrap.DataAccess
{
    public class Task
    {
        /// <summary>
        /// 获取/设置  任务ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获取/设置  任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 获取/设置  分配人
        /// </summary>
        public string AssignName { get; set; }
        /// <summary>
        /// 获取/设置  完成任务人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 获取/设置  任务所需时间（天）
        /// </summary>
        public int TaskTime { get; set; }
        /// <summary>
        /// 获取/设置  任务进度
        /// </summary>
        public double TaskProgress { get; set; }
        /// <summary>
        /// 获取/设置  分配时间
        /// </summary>
        public DateTime AssignTime { get; set; }
    }
}
