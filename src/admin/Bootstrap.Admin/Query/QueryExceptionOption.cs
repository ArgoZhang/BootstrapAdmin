using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Linq;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryExceptionOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<object> Retrieves()
        {
            var data = ExceptionsHelper.RetrievePages(this, StartTime, EndTime);
            var ret = new QueryData<object>();
            ret.total = (int)data.TotalItems;
            ret.rows = data.Items.Select(ex => new { ex.UserId, ex.UserIp, ex.LogTime, ex.Message, ex.ErrorPage, ex.ExceptionType });
            return ret;
        }
    }
}
