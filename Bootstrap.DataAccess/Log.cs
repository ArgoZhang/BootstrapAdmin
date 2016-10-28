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
        /// 获得/设置 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 获得/设置 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 获得/设置 操作表表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 获得/设置 操作内容
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 获得/设置 操作表的主键
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 获得/设置 sql语句
        /// </summary>
        public string SqlText { get; set; }

        /// <summary>
        /// 获得/设置 操作者Ip
        /// </summary>
        public string OperationIp { get; set; }
    }
}
