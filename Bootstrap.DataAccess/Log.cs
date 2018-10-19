using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    public class Log
    {
        protected const string RetrieveLogsDataKey = "LogHelper-RetrieveLogs";
        /// <summary>
        /// 获得/设置 操作日志主键ID
        /// </summary>
        public int Id { get; set; }

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
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Log> RetrieveLogs(string tId = null) => throw new NotImplementedException();
        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveLog(Log p) => throw new NotImplementedException();
    }
}
