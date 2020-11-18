using Bootstrap.Admin.Pages.Components;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 菜单维护组件
    /// </summary>
    public class MenusBase : QueryPageBase<BootstrapMenu>
    {
        /// <summary>
        /// 获得 授权服务
        /// </summary>
        [Inject]
        protected AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        /// <summary>
        /// 获得/设置 Modal 实例
        /// </summary>
        protected AssignModalBase<Role>? AssignRoleModal { get; set; }

        /// <summary>
        /// 获得/设置 菜单类别
        /// </summary>
        protected List<SelectedItem> QueryCategory { get; set; } = new List<SelectedItem>(new SelectedItem[] {
            new SelectedItem() { Text = "全部", Value = "", Active = true },
            new SelectedItem() { Text = "系统菜单", Value = "0" },
            new SelectedItem() { Text = "外部菜单", Value = "1" }
        });

        /// <summary>
        /// 获得/设置 菜单类型
        /// </summary>
        protected List<SelectedItem> QueryResource { get; set; } = new List<SelectedItem>(new SelectedItem[] {
            new SelectedItem() { Text = "全部", Value = "-1", Active = true },
            new SelectedItem() { Text = "菜单", Value = "0" },
            new SelectedItem() { Text = "资源", Value = "1" },
            new SelectedItem() { Text = "按钮", Value = "2" }
        });

        /// <summary>
        /// 获得/设置 所属应用
        /// </summary>
        protected List<SelectedItem> QueryApp { get; set; } = new List<SelectedItem>(new SelectedItem[] {
            new SelectedItem() { Text = "全部", Value = "", Active = true }
        });


        /// <summary>
        /// 获得/设置 菜单类别
        /// </summary>
        protected List<SelectedItem> DefineCategory { get; set; } = new List<SelectedItem>(new SelectedItem[] {
            new SelectedItem() { Text = "系统菜单", Value = "0" },
            new SelectedItem() { Text = "外部菜单", Value = "1" }
        });

        /// <summary>
        /// 获得/设置 菜单类型
        /// </summary>
        protected List<SelectedItem> DefineResource { get; set; } = new List<SelectedItem>(new SelectedItem[] {
            new SelectedItem() { Text = "菜单", Value = "0" },
            new SelectedItem() { Text = "资源", Value = "1" },
            new SelectedItem() { Text = "按钮", Value = "2" }
        });

        /// <summary>
        /// 获得/设置 所属应用
        /// </summary>
        protected List<SelectedItem> DefineApp { get; set; } = new List<SelectedItem>();

        /// <summary>
        /// 获得/设置 所属应用
        /// </summary>
        protected List<SelectedItem> DefineTarget { get; set; } = new List<SelectedItem>() {
            new SelectedItem() { Text = "本窗口", Value = "_self" },
            new SelectedItem() { Text = "新窗口", Value = "_blank" },
            new SelectedItem() { Text = "父级窗口", Value = "_parent" },
            new SelectedItem() { Text = "顶级窗口", Value = "_top" }
        };

        /// <summary>
        /// 获得/设置 用户登录名
        /// </summary>
        protected string? UserName { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (AuthenticationStateProvider != null)
            {
                var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                UserName = state?.User.Identity!.Name;
            }
        }

        /// <summary>
        /// SetParametersAsync 方法
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            QueryModel.Category = "";
            QueryModel.IsResource = -1;
            QueryApp.AddRange(DictHelper.RetrieveApps().Select(app => new SelectedItem() { Text = app.Value, Value = app.Key }));
            DefineApp.AddRange(DictHelper.RetrieveApps().Select(app => new SelectedItem() { Text = app.Value, Value = app.Key }));
            return base.SetParametersAsync(ParameterView.Empty);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="options"></param>
        protected override QueryData<BootstrapMenu> Query(QueryPageOptions options)
        {
            var data = MenuHelper.RetrieveMenusByUserName(UserName);
            if (!string.IsNullOrEmpty(QueryModel.Name)) data = data.Where(d => d.Name.Contains(QueryModel.Name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.ParentName)) data = data.Where(d => d.ParentName.Contains(QueryModel.ParentName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.Category)) data = data.Where(d => d.Category == QueryModel.Category);
            if (QueryModel.IsResource != -1) data = data.Where(d => d.IsResource == QueryModel.IsResource);
            if (!string.IsNullOrEmpty(QueryModel.Application)) data = data.Where(d => d.Application.Equals(QueryModel.Application, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(options.SearchText)) data = data.Where(d => d.Name.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || d.ParentName.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || d.Category.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || d.Application.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase));
            var totalCount = data.Count();
            var items = data.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems);
            return new QueryData<BootstrapMenu>() { Items = items, TotalCount = totalCount, PageIndex = options.PageIndex, PageItems = options.PageItems };
        }

        /// <summary>
        /// 新建方法
        /// </summary>
        protected override BootstrapMenu Add()
        {
            return new BootstrapMenu()
            {
                Order = 10,
                Icon = "fa fa-fa",
                Target = "_self",
                Category = "0",
                IsResource = 0,
                Application = DefineApp.First().Value
            };
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        protected override bool Save(BootstrapMenu item) => MenuHelper.Save(item);

        /// <summary>
        /// 删除方法
        /// </summary>
        protected override bool Delete(IEnumerable<BootstrapMenu> items) => MenuHelper.Delete(items.Select(item => item.Id ?? ""));

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {
            QueryModel.Name = "";
            QueryModel.ParentName = "";
            QueryModel.Category = "";
            QueryModel.IsResource = -1;
            QueryModel.Application = "";
        }

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
                    ShowMessage("角色授权", "请选择一个菜单", ToastCategory.Information);
                }
                else
                {
                    var menuId = EditPage.SelectedItems.First().Id;
                    if (!string.IsNullOrEmpty(menuId))
                    {
                        var roles = RoleHelper.RetrievesByMenuId(menuId);
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
                var menuId = EditPage.SelectedItems.First().Id;
                var roleIds = roles.Where(r => r.Checked == "checked").Select(r => r.Id ?? "");
                if (!string.IsNullOrEmpty(menuId)) ret = RoleHelper.SavaByMenuId(menuId, roleIds);
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
    }
}
