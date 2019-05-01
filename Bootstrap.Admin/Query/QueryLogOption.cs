using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryLogOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string OperateType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? OperateTimeStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? OperateTimeEnd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<Log> RetrieveData()
        {
            var data = LogHelper.Retrieves(this, OperateTimeStart, OperateTimeEnd, OperateType);
            var ret = new QueryData<Log>();
            ret.total = data.TotalItems;
            ret.rows = data.Items;
            return ret;
        }
    }
}