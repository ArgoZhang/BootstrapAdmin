using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Globalization;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class QueryExceptionOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string OperateTimeStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OperateTimeEnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<Exceptions> RetrieveData()
        {
            var data = ExceptionHelper.RetrieveExceptions();

            if (!string.IsNullOrEmpty(OperateTimeStart))
            {
                DateTime opTimeStart = StringToDateTime(OperateTimeStart);
                if (opTimeStart != null)
                    data = data.Where(t => IsSmallThen(opTimeStart, t.LogTime));
            }
            if (!string.IsNullOrEmpty(OperateTimeEnd))
            {
                DateTime opTimeEnd = StringToDateTime(OperateTimeEnd);
                if (opTimeEnd != null)
                    data = data.Where(t => IsSmallThen(t.LogTime, opTimeEnd));
            }

            var ret = new QueryData<Exceptions>();
            ret.total = data.Count();
            data = Order == "asc" ? data.OrderBy(t => t.AppDomainName) : data.OrderByDescending(t => t.AppDomainName);
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
        private static DateTime StringToDateTime(string dt_str)
        {
            DateTime dt;
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
            dt = Convert.ToDateTime(dt_str, dtFormat);
            return dt;
        }
        /// <summary>
        /// 比较两个DateTime
        /// (去掉了毫秒)
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private static bool IsSmallThen(DateTime d1, DateTime d2)
        {
            return new DateTime(d1.Year, d1.Month, d1.Day, d1.Hour, d1.Minute, d1.Second) <=
                new DateTime(d2.Year, d2.Month, d2.Day, d2.Hour, d2.Minute, d2.Second);
        }
    }
}
