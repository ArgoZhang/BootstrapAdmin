using Bootstrap.Admin.Pages.Components;
using Bootstrap.Admin.Pages.Models;
using Bootstrap.Admin.Pages.Shared;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 个人中心组件类
    /// </summary>
    public class ProfilesBase : PageBase
    {
        /// <summary>
        /// 获得/设置 RootLayout 实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout? RootLayout { get; protected set; }

        /// <summary>
        /// 获得/设置 ProfilesModel 实例
        /// </summary>
        protected ProfilesModel? Model { get; set; }

        /// <summary>
        /// 获得/设置 是否为演示系统
        /// </summary>
        protected bool IsDemo { get; set; } = false;

        /// <summary>
        /// 获得/设置 BootstrapUser 实例
        /// </summary>
        protected BootstrapUser User { get; set; } = new BootstrapUser();

        /// <summary>
        /// 获得/设置  PasswModel 实例
        /// </summary>
        protected PasswordModel Password { get; set; } = new PasswordModel();

        /// <summary>
        /// 获得/设置  当前用户显示名称
        /// </summary>
        [DisplayName("显示名称")]
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// IJSRuntime 接口实例
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 获得/设置 选中的样式项
        /// </summary>
        protected SelectedItem SelectedTheme { get; set; } = new SelectedItem();

        /// <summary>
        /// 获得/设置 选中的应用程序
        /// </summary>
        protected SelectedItem SelectedApp { get; set; } = new SelectedItem();

        /// <summary>
        /// 获得/设置 应用程序集合
        /// </summary>
        protected IEnumerable<SelectedItem> Apps { get; set; } = new SelectedItem[0];

        /// <summary>
        /// 获得/设置 网站样式集合
        /// </summary>
        protected IEnumerable<SelectedItem> Themes { get; set; } = new SelectedItem[0];

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ret"></param>
        protected void ShowMessage(string text, bool ret = true) => JSRuntime?.ShowToast("个人中心", text, ret ? ToastCategory.Success : ToastCategory.Error);

        /// <summary>
        /// 组件初始化方法
        /// </summary>
        protected override void OnInitialized()
        {
            Model = new ProfilesModel(RootLayout?.UserName);
            Themes = new SelectedItem[] { new SelectedItem() { Text = "默认样式" } }.Union(Model.Themes.Select(t => new SelectedItem() { Text = t.Name, Value = t.Code }));
            Apps = Model.Applications.Select(t => new SelectedItem() { Text = t.Value, Value = t.Key });

            SelectedTheme = Themes.First();
            SelectedApp = Apps.First();

            var user = UserHelper.RetrieveUserByUserName(Model?.UserName);
            if (user != null) User = user;

            // 直接绑定 User.DisplayName 导致未保存时 UI 的显示名称也会变化
            DisplayName = User.DisplayName;
        }

        /// <summary>
        /// 保存显示名称方法
        /// </summary>
        protected void SaveDisplayName(EditContext context)
        {
            if (!string.IsNullOrEmpty(User.UserName))
            {
                var ret = UserHelper.SaveDisplayName(User.UserName, DisplayName);
                if (ret)
                {
                    User.DisplayName = DisplayName;
                    RootLayout?.OnDisplayNameChanged(DisplayName);
                }

                // 弹窗提示是否保存成功
                var result = ret ? "成功" : "失败";
                ShowMessage($"保存显示名称{result}", ret);
            }
        }

        /// <summary>
        /// 保存密码方法
        /// </summary>
        protected void SavePassword(EditContext context)
        {
            var ret = UserHelper.ChangePassword(User.UserName, Password.Password, Password.NewPassword);

            // 弹窗提示是否保存成功
            var result = ret ? "成功" : "失败";
            ShowMessage($"更新密码{result}", ret);
        }

        /// <summary>
        /// 保存默认应用方法
        /// </summary>
        protected void SaveApp()
        {
            var ret = UserHelper.SaveApp(User.UserName, SelectedApp.Value);

            // 弹窗提示是否保存成功
            var result = ret ? "成功" : "失败";
            ShowMessage($"保存默认应用{result}", ret);
        }

        /// <summary>
        /// 保存网站样式方法
        /// </summary>
        protected void SaveTheme()
        {
            var ret = UserHelper.SaveUserCssByName(User.UserName, SelectedTheme.Value);

            // 弹窗提示是否保存成功
            var result = ret ? "成功" : "失败";
            ShowMessage($"保存网站样式{result}", ret);
        }

        /// <summary>
        /// 密码保存实体类
        /// </summary>
        protected class PasswordModel
        {
            /// <summary>
            /// 获得/设置 原密码
            /// </summary>
            [DisplayName("原密码")]
            public string Password { get; set; } = "";

            /// <summary>
            /// 获得/设置 新密码
            /// </summary>
            [DisplayName("新密码")]
            public string NewPassword { get; set; } = "";

            /// <summary>
            /// 获得/设置 确认密码
            /// </summary>
            [DisplayName("确认密码")]
            public string ConfirmPassword { get; set; } = "";
        }
    }
}
