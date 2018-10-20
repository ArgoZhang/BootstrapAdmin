using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Linq;

namespace Bootstrap.Admin.Query
{
    public class QueryLogOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string OperateType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OperateTimeStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OperateTimeEnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<Log> RetrieveData()
        {
            var data = LogHelper.RetrieveLogs();
            if (!string.IsNullOrEmpty(OperateType))
            {
                data = data.Where(t => t.CRUD.ToString().Contains(OperateType));
            }

            if (OperateTimeStart > DateTime.MinValue)
            {
                data = data.Where(t => t.LogTime > OperateTimeStart);
            }
            if (OperateTimeEnd > DateTime.MinValue)
            {
                data = data.Where(t => t.LogTime < OperateTimeEnd.AddDays(1));
            }

            var ret = new QueryData<Log>();
            ret.total = data.Count();
            switch (Sort)
            {
                case "CRUD":
                    data = Order == "asc" ? data.OrderBy(t => t.CRUD) : data.OrderByDescending(t => t.CRUD);
                    break;
                case "UserName":
                    data = Order == "asc" ? data.OrderBy(t => t.UserName) : data.OrderByDescending(t => t.UserName);
                    break;
                case "LogTime":
                    data = Order == "asc" ? data.OrderBy(t => t.LogTime) : data.OrderByDescending(t => t.LogTime);
                    break;
                case "ClientIp":
                    data = Order == "asc" ? data.OrderBy(t => t.ClientIp) : data.OrderByDescending(t => t.ClientIp);
                    break;
                case "RequestUrl":
                    data = Order == "asc" ? data.OrderBy(t => t.RequestUrl) : data.OrderByDescending(t => t.RequestUrl);
                    break;
                default:
                    break;
            }
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}