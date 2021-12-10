using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Linq;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 角色查询条件类
    /// </summary>
    public class QueryRoleOption : PaginationOption
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 角色数据
        /// </summary>
        /// <returns></returns>
        public QueryData<object> RetrieveData()
        {
            // int limit, int offset, string name, string price, string sort, string order
            var data = RoleHelper.Retrieves();
            if (!string.IsNullOrEmpty(RoleName))
            {
                data = data.Where(t => t.RoleName.Contains(RoleName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(Description))
            {
                data = data.Where(t => t.Description.Contains(Description, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(Search))
            {
                data = data.Where(t => t.RoleName.Contains(Search, StringComparison.OrdinalIgnoreCase) || t.Description.Contains(Search, StringComparison.OrdinalIgnoreCase));
            }
            var ret = new QueryData<object>();
            ret.total = data.Count();
            data = Order == "asc" ? data.OrderBy(t => t.RoleName) : data.OrderByDescending(t => t.RoleName);
            ret.rows = data.Skip(Offset).Take(Limit).Select(r => new { r.Id, r.RoleName, r.Description });
            return ret;
        }
    }
}
