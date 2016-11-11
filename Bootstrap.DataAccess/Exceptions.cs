using System;
namespace Bootstrap.DataAccess
{
    public class Exceptions
    {
        public int ID { get; set; }
        public string AppDomainName { get; set; }
        public string ErrorPage { get; set; }
        public string UserID { get; set; }
        public string UserIp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime LogTime { get; set; }
    }
}
