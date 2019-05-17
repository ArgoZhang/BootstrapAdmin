using System;

namespace Bootstrap.DataAccess.MongoDB
{
    public class RejectUser
    {
        /// <summary>
        /// 获得/设置 操作日志主键ID
        /// </summary>
        public string Id { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public DateTime RegisterTime { get; set; }

        public string RejectedBy { get; set; }

        public DateTime RejectedTime { get; set; }

        public string RejectedReason { get; set; }
    }
}
