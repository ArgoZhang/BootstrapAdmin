using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Globalization;
using System.Linq;

namespace Bootstrap.Admin.Query
{
    public class QueryExceptionOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<Exceptions> RetrieveData()
        {
            var data = ExceptionsHelper.RetrieveExceptions();
            if (StartTime > DateTime.MinValue)
            {
                data = data.Where(t => t.LogTime > StartTime);
            }
            if (EndTime > DateTime.MinValue)
            {
                data = data.Where(t => t.LogTime < EndTime.AddDays(1));
            }
            var ret = new QueryData<Exceptions>();
            ret.total = data.Count();
            switch (Sort)
            {
                case "ErrorPage":
                    data = Order == "asc" ? data.OrderBy(t => t.ErrorPage) : data.OrderByDescending(t => t.ErrorPage);
                    break;
                case "UserID":
                    data = Order == "asc" ? data.OrderBy(t => t.UserId) : data.OrderByDescending(t => t.UserId);
                    break;
                case "UserIp":
                    data = Order == "asc" ? data.OrderBy(t => t.UserIp) : data.OrderByDescending(t => t.UserIp);
                    break;
                case "LogTime":
                    data = Order == "asc" ? data.OrderBy(t => t.LogTime) : data.OrderByDescending(t => t.LogTime);
                    break;
                default:
                    break;
            }
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}
