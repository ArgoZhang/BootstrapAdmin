using Bootstrap.Admin.Components;
using Bootstrap.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Pages.Admin.Components
{
    /// <summary>
    /// 用户表维护组件
    /// </summary>
    public class UsersBase : QueryPageBase<User>
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageItems">每页显示数据条目数量</param>
        protected override QueryData<User> Query(int pageIndex, int pageItems)
        {
            var data = UserHelper.Retrieves();
            if (!string.IsNullOrEmpty(QueryModel.UserName)) data = data.Where(d => d.UserName.Contains(QueryModel.UserName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.DisplayName)) data = data.Where(d => d.DisplayName.Contains(QueryModel.DisplayName, StringComparison.OrdinalIgnoreCase));
            var totalCount = data.Count();
            var items = data.Skip((pageIndex - 1) * pageItems).Take(pageItems);
            return new QueryData<User>() { Items = items, TotalCount = totalCount, PageIndex = pageIndex, PageItems = pageItems };
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
