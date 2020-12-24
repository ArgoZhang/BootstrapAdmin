using Bootstrap.Admin.Pages.Components;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        /// 获得/设置 Modal 实例
        /// </summary>
        protected AssignModalBase<Group>? AssignGroupModal { get; set; }

        /// <summary>
        /// 弹窗分配角色方法
        /// </summary>
        protected void AssignGroups()
        {
            // 菜单对角色授权操作
            if (EditPage != null)
            {
                if (EditPage.SelectedItems.Count() != 1)
                {
                    ShowMessage("部门授权", "请选择一个用户", ToastCategory.Information);
                }
                else
                {
                    var userId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var groups = GroupHelper.RetrievesByUserId(userId);
                        AssignGroupModal?.Update(groups);
                    }
                }
            }
        }

        /// <summary>
        /// 保存授权部门方法
        /// </summary>
        protected void SaveGroups(IEnumerable<Group> groups)
        {
            bool ret = false;
            if (EditPage != null && EditPage.SelectedItems.Any())
            {
                var userId = EditPage.SelectedItems.First().Id;
                var groupIds = groups.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(userId)) ret = GroupHelper.SaveByUserId(userId, groupIds);
            }
            ShowMessage("部门授权", ret ? "保存成功" : "保存失败", ret ? ToastCategory.Success : ToastCategory.Error);
        }

        /// <summary>
        /// 选择框点击时调用此方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="check"></param>
        protected void OnGroupClick(Group item, bool check)
        {
            item.Checked = check ? "checked" : "";
        }

        /// <summary>
        /// 设置初始化值
        /// </summary>
        protected CheckBoxState SetGroupCheck(Group item) => item.Checked == "checked" ? CheckBoxState.Checked : CheckBoxState.UnChecked;

        /// <summary>
        /// IJSRuntime 接口实例
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="cate"></param>
        protected void ShowMessage(string title, string text, ToastCategory cate = ToastCategory.Success) => JSRuntime?.ShowToast(title, text, cate);

        /// <summary>
        /// 获得/设置 Modal 实例
        /// </summary>
        protected AssignModalBase<Role>? AssignRoleModal { get; set; }

        /// <summary>
        /// 弹窗分配角色方法
        /// </summary>
        protected void AssignRoles()
        {
            // 菜单对角色授权操作
            if (EditPage != null)
            {
                if (EditPage.SelectedItems.Count() != 1)
                {
                    ShowMessage("角色授权", "请选择一个用户", ToastCategory.Information);
                }
                else
                {
                    var userId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var roles = RoleHelper.RetrievesByUserId(userId);
                        AssignRoleModal?.Update(roles);
                    }
                }
            }
        }

        /// <summary>
        /// 保存授权角色方法
        /// </summary>
        protected void SaveRoles(IEnumerable<Role> roles)
        {
            bool ret = false;
            if (EditPage != null && EditPage.SelectedItems.Any())
            {
                var userId = EditPage.SelectedItems.First().Id;
                var roleIds = roles.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(userId)) ret = RoleHelper.SaveByUserId(userId, roleIds);
            }
            ShowMessage("角色授权", ret ? "保存成功" : "保存失败", ret ? ToastCategory.Success : ToastCategory.Error);
        }

        /// <summary>
        /// 选择框点击时调用此方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="check"></param>
        protected void OnClick(Role item, bool check)
        {
            item.Checked = check ? "checked" : "";
        }

        /// <summary>
        /// 设置初始化值
        /// </summary>
        protected CheckBoxState SetCheck(Role item) => item.Checked == "checked" ? CheckBoxState.Checked : CheckBoxState.UnChecked;
    }
}
