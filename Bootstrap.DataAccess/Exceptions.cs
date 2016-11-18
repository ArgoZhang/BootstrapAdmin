using System;
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
        public int ID { get; set; }
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
        public string UserID { get; set; }
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
    }
}
