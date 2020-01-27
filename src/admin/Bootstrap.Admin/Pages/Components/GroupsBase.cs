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
        /// <param name="options"></param>
        protected override QueryData<Group> Query(QueryPageOptions options)
        {
            var data = GroupHelper.Retrieves();
            if (!string.IsNullOrEmpty(QueryModel.GroupName)) data = data.Where(d => d.GroupName.Contains(QueryModel.GroupName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.Description)) data = data.Where(d => d.Description != null && d.Description.Contains(QueryModel.Description, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(options.SearchText)) data = data.Where(d => d.GroupName.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || (d.Description ?? "").Contains(options.SearchText, StringComparison.OrdinalIgnoreCase));
            var totalCount = data.Count();
            var items = data.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems);
            return new QueryData<Group>() { Items = items, TotalCount = totalCount, PageIndex = options.PageIndex, PageItems = options.PageItems };
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
