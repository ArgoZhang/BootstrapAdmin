using System;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class RejectUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RejectedBy { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public DateTime RejectedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RejectedReason { get; set; } = "";
    }
}
