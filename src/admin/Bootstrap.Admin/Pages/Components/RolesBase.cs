using Bootstrap.Admin.Components;
using Bootstrap.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Pages.Admin.Components
{
    /// <summary>
    /// 部门维护组件
    /// </summary>
    public class RolesBase : QueryPageBase<Role>
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="options"></param>
        protected override QueryData<Role> Query(QueryPageOptions options)
        {
            var data = RoleHelper.Retrieves();
            if (!string.IsNullOrEmpty(QueryModel.RoleName)) data = data.Where(d => d.RoleName.Contains(QueryModel.RoleName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.Description)) data = data.Where(d => d.Description != null && d.Description.Contains(QueryModel.Description, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(options.SearchText)) data = data.Where(d => d.RoleName.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || d.Description.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase));
            var totalCount = data.Count();
            var items = data.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems);
            return new QueryData<Role>() { Items = items, TotalCount = totalCount, PageIndex = options.PageIndex, PageItems = options.PageItems };
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        protected override bool Save(Role item) => RoleHelper.Save(item);

        /// <summary>
        /// 删除方法
        /// </summary>
        protected override bool Delete(IEnumerable<Role> items) => RoleHelper.Delete(items.Select(item => item.Id ?? ""));

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {
            QueryModel.RoleName = "";
            QueryModel.Description = "";
        }

        /// <summary>
        /// 分配用户方法
        /// </summary>
        protected void AssignUsers()
        {

        }

        /// <summary>
        /// 分配部门方法
        /// </summary>
        protected void AssignGroups()
        {

        }

        /// <summary>
        /// 分配菜单方法
        /// </summary>
        protected void AssignMenus()
        {

        }

        /// <summary>
        /// 分配应用方法
        /// </summary>
        protected void AssignApps()
        {

        }
    }
}
