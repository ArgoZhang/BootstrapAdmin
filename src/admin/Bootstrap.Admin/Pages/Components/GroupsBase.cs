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
    public class GroupsBase : QueryPageBase<Group>
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageItems">每页显示数据条目数量</param>
        /// <param name="searchText"></param>
        protected override QueryData<Group> Query(int pageIndex, int pageItems, string searchText)
        {
            var data = GroupHelper.Retrieves();
            if (!string.IsNullOrEmpty(QueryModel.GroupName)) data = data.Where(d => d.GroupName.Contains(QueryModel.GroupName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.Description)) data = data.Where(d => d.Description != null && d.Description.Contains(QueryModel.Description, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(searchText)) data = data.Where(d => d.GroupName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || d.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            var totalCount = data.Count();
            var items = data.Skip((pageIndex - 1) * pageItems).Take(pageItems);
            return new QueryData<Group>() { Items = items, TotalCount = totalCount, PageIndex = pageIndex, PageItems = pageItems };
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        protected override bool Save(Group group) => GroupHelper.Save(group);

        /// <summary>
        /// 删除方法
        /// </summary>
        protected override bool Delete(IEnumerable<Group> groups) => GroupHelper.Delete(groups.Select(item => item.Id ?? ""));

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {
            QueryModel.GroupName = "";
            QueryModel.Description = "";
        }

        /// <summary>
        /// 分配用户方法
        /// </summary>
        protected void AssignUsers()
        {

        }

        /// <summary>
        /// 分配角色方法
        /// </summary>
        protected void AssignRoles()
        {

        }
    }
}
