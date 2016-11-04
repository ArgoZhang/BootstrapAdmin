using System;
namespace Bootstrap.DataAccess
{
    public class Log
    {
        /// <summary>
        /// 获得/设置 操作日志主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 获得/设置 操作类型
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// 获得/设置 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获得/设置 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 获得/设置 操作者Ip
        /// </summary>
        public string OperationIp { get; set; }

        /// <summary>
        /// 获取/设置 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 获取/设置 操作模块
        /// </summary>
        public string OperationModule { get; set; }
    }
}
