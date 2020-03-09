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

            // sort
            data = options.SortName switch
            {
                nameof(Group.GroupName) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.GroupName) : data.OrderByDescending(d => d.GroupName),
                nameof(Group.GroupCode) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.GroupCode) : data.OrderByDescending(d => d.GroupCode),
                nameof(Group.Description) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.Description) : data.OrderByDescending(d => d.Description),
                _ => data
            };

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
        /// 获得/设置 Modal 实例
        /// </summary>
        protected AssignModalBase<User>? AssignUserModal { get; set; }

        /// <summary>
        /// 弹窗分配角色方法
        /// </summary>
        protected void AssignUsers()
        {
            // 菜单对角色授权操作
            if (EditPage != null)
            {
                if (EditPage.SelectedItems.Count() != 1)
                {
                    ShowMessage("用户授权", "请选择一个部门", ToastCategory.Information);
                }
                else
                {
                    var groupId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(groupId))
                    {
                        var users = UserHelper.RetrievesByGroupId(groupId);
                        AssignUserModal?.Update(users);
                    }
                }
            }
        }

        /// <summary>
        /// 保存授权部门方法
        /// </summary>
        protected void SaveUsers(IEnumerable<User> users)
        {
            bool ret = false;
            if (EditPage != null && EditPage.SelectedItems.Any())
            {
                var groupId = EditPage.SelectedItems.First().Id;
                var userIds = users.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(groupId)) ret = UserHelper.SaveByGroupId(groupId, userIds);
            }
            ShowMessage("用户授权", ret ? "保存成功" : "保存失败", ret ? ToastCategory.Success : ToastCategory.Error);
        }

        /// <summary>
        /// 选择框点击时调用此方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="check"></param>
        protected void OnUserClick(User item, bool check)
        {
            item.Checked = check ? "checked" : "";
        }

        /// <summary>
        /// 设置初始化值
        /// </summary>
        protected CheckBoxState SetUserCheck(User item) => item.Checked == "checked" ? CheckBoxState.Checked : CheckBoxState.UnChecked;

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
                    ShowMessage("角色授权", "请选择一个部门", ToastCategory.Information);
                }
                else
                {
                    var groupId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(groupId))
                    {
                        var roles = RoleHelper.RetrievesByGroupId(groupId);
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
                var groupId = EditPage.SelectedItems.First().Id;
                var roleIds = roles.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(groupId)) ret = RoleHelper.SaveByGroupId(groupId, roleIds);
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
