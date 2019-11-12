using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 操作日志查询条件类
    /// </summary>
    public class QueryLogOption : PaginationOption
    {
        /// <summary>
        /// 获得/设置 操作类型
        /// </summary>
        public string? OperateType { get; set; }

        /// <summary>
        /// 获得/设置 开始时间
        /// </summary>
        public DateTime? OperateTimeStart { get; set; }

        /// <summary>
        /// 获得/设置 结束时间
        /// </summary>
        public DateTime? OperateTimeEnd { get; set; }

        /// <summary>
        /// 获得/设置 获取查询分页数据
        /// </summary>
        /// <returns></returns>
        public QueryData<Log> RetrieveData()
        {
            var data = LogHelper.RetrievePages(this, OperateTimeStart, OperateTimeEnd, OperateType);
            var ret = new QueryData<Log>();
            ret.total = data.TotalItems;
            ret.rows = data.Items;
            return ret;
        }
    }
}
