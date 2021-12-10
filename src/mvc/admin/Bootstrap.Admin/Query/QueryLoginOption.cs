using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 登录日志查询条件 
    /// </summary>
    public class QueryLoginOption : PaginationOption
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
        /// 登录IP地址
        /// </summary>
        public string? LoginIP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<LoginUser> RetrieveData()
        {
            var data = LoginHelper.RetrievePages(this, StartTime, EndTime, LoginIP);
            return new QueryData<LoginUser>
            {
                total = data.TotalItems,
                rows = data.Items
            };
        }
    }
}
