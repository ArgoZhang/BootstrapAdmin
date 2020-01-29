using Bootstrap.Admin.Pages.Components;
using Bootstrap.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 用户表维护组件
    /// </summary>
    public class UsersBase : QueryPageBase<User>
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="options"></param>
        protected override QueryData<User> Query(QueryPageOptions options)
        {
            var data = UserHelper.Retrieves();
            if (!string.IsNullOrEmpty(QueryModel.UserName)) data = data.Where(d => d.UserName.Contains(QueryModel.UserName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.DisplayName)) data = data.Where(d => d.DisplayName.Contains(QueryModel.DisplayName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(options.SearchText)) data = data.Where(d => d.UserName.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || d.DisplayName.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase));
            
            // sort
            data = options.SortName switch
            {
                nameof(User.UserName) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.UserName) : data.OrderByDescending(d => d.UserName),
                nameof(User.DisplayName) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.DisplayName) : data.OrderByDescending(d => d.DisplayName),
                nameof(User.Description) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.Description) : data.OrderByDescending(d => d.Description),
                nameof(User.ApprovedBy) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.ApprovedBy) : data.OrderByDescending(d => d.ApprovedBy),
                nameof(User.ApprovedTime) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.ApprovedTime) : data.OrderByDescending(d => d.ApprovedTime),
                nameof(User.RegisterTime) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.RegisterTime) : data.OrderByDescending(d => d.RegisterTime),
                _ => data
            };

            var totalCount = data.Count();
            var items = data.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems);
            return new QueryData<User>() { Items = items, TotalCount = totalCount, PageIndex = options.PageIndex, PageItems = options.PageItems };
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        protected override bool Save(User user) => UserHelper.Save(user);

        /// <summary>
        /// 删除方法
        /// </summary>
        protected override bool Delete(IEnumerable<User> users) => UserHelper.Delete(users.Select(item => item.Id ?? ""));

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {
            QueryModel.UserName = "";
            QueryModel.DisplayName = "";
        }

        /// <summary>
        /// 分配部门方法
        /// </summary>
        protected void AssignGroups()
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
