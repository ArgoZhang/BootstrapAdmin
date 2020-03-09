using Bootstrap.Admin.Pages.Components;
using Bootstrap.Admin.Pages.Shared;
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

            // sort
            data = options.SortName switch
            {
                nameof(Role.RoleName) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.RoleName) : data.OrderByDescending(d => d.RoleName),
                nameof(Role.Description) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.Description) : data.OrderByDescending(d => d.Description),
                _ => data
            };

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
                    ShowMessage("用户授权", "请选择一个角色", ToastCategory.Information);
                }
                else
                {
                    var roleId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(roleId))
                    {
                        var users = UserHelper.RetrievesByRoleId(roleId);
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
                var roleId = EditPage.SelectedItems.First().Id;
                var userIds = users.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(roleId)) ret = UserHelper.SaveByRoleId(roleId, userIds);
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
                    ShowMessage("部门授权", "请选择一个角色", ToastCategory.Information);
                }
                else
                {
                    var roleId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(roleId))
                    {
                        var groups = GroupHelper.RetrievesByRoleId(roleId);
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
                var roleId = EditPage.SelectedItems.First().Id;
                var groupIds = groups.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(roleId)) ret = GroupHelper.SaveByRoleId(roleId, groupIds);
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
        /// 获得/设置 Modal 实例
        /// </summary>
        protected AssignModalBase<DataAccess.App>? AssignAppModal { get; set; }

        /// <summary>
        /// 弹窗分配角色方法
        /// </summary>
        protected void AssignApps()
        {
            // 菜单对角色授权操作
            if (EditPage != null)
            {
                if (EditPage.SelectedItems.Count() != 1)
                {
                    ShowMessage("应用程序授权", "请选择一个角色", ToastCategory.Information);
                }
                else
                {
                    var roleId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(roleId))
                    {
                        var apps = AppHelper.RetrievesByRoleId(roleId);
                        AssignAppModal?.Update(apps);
                    }
                }
            }
        }

        /// <summary>
        /// 保存授权部门方法
        /// </summary>
        protected void SaveApps(IEnumerable<DataAccess.App> apps)
        {
            bool ret = false;
            if (EditPage != null && EditPage.SelectedItems.Any())
            {
                var roleId = EditPage.SelectedItems.First().Id;
                var appIds = apps.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(roleId)) ret = AppHelper.SaveByRoleId(roleId, appIds);
            }
            ShowMessage("应用程序授权", ret ? "保存成功" : "保存失败", ret ? ToastCategory.Success : ToastCategory.Error);
        }

        /// <summary>
        /// 选择框点击时调用此方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="check"></param>
        protected void OnAppClick(DataAccess.App item, bool check)
        {
            item.Checked = check ? "checked" : "";
        }

        /// <summary>
        /// 设置初始化值
        /// </summary>
        protected CheckBoxState SetAppCheck(DataAccess.App item) => item.Checked == "checked" ? CheckBoxState.Checked : CheckBoxState.UnChecked;

        /// <summary>
        /// 获得/设置 Modal 实例
        /// </summary>
        protected AssignModalBase<Menu>? AssignMenuModal { get; set; }

        /// <summary>
        /// 弹窗分配菜单方法
        /// </summary>
        protected void AssignMenus()
        {

        }
    }
}
