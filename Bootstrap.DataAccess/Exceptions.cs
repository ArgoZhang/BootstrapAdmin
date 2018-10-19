using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Exceptions
    {
        /// <summary>
        /// 
        /// </summary>
        protected const string RetrieveExceptionsDataKey = "ExceptionHelper-RetrieveExceptions";
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppDomainName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LogTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExceptionType { get; set; }
        /// <summary>
        /// 获得/设置 时间描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public virtual void Log(Exception ex, NameValueCollection additionalInfo) => throw new NotImplementedException();
        /// <summary>
        /// 查询一周内所有异常
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Exceptions> RetrieveExceptions() => throw new NotImplementedException();
    }
}
