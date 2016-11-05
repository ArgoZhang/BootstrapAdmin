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
        public string CRUD { get; set; }

        /// <summary>
        /// 获得/设置 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获得/设置 操作时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 获得/设置 客户端IP
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 获取/设置 客户端信息
        /// </summary>
        public string ClientAgent { get; set; }

        /// <summary>
        /// 获取/设置 请求网址
        /// </summary>
        public string RequestUrl { get; set; }
    }
}
