using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ProcessTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProcessBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProcessResult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 获得/设置 市场描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }
    }
}
