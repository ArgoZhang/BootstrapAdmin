using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Linq;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 程序异常查询条件类
    /// </summary>
    public class QueryExceptionOption : PaginationOption
    {
        /// <summary>
        /// 获得/设置 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 获得/设置 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 查询方法
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
