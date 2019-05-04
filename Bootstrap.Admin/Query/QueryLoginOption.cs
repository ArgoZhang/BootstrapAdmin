using Bootstrap.DataAccess;
using Longbow.Web.Mvc;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 登录日志查询条件 
    /// </summary>
    public class QueryLoginOption : PaginationOption
    {
        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<LoginUser> RetrieveData()
        {
            var data = LoginHelper.Retrieves(this, LoginIP);
            return new QueryData<LoginUser>
            {
                total = data.TotalItems,
                rows = data.Items
            };
        }
    }
}
