using System.ComponentModel;
using Bootstrap.Admin.Models;
using Bootstrap.Admin.Shared;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Bootstrap.Pages.Admin.Components
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
        protected string DisplayName { get; set; } = "";

        /// <summary>
        /// 组件初始化方法
        /// </summary>
        protected override void OnInitialized()
        {
            Model = new ProfilesModel(RootLayout?.UserName);
            var user = DataAccess.UserHelper.RetrieveUserByUserName(Model?.UserName);
            if (user != null) User = user;

            // 直接绑定 User.DisplayName 导致未保存时 UI 的显示名称也会变化
            DisplayName = User.DisplayName;
        }

        /// <summary>
        /// 保存显示名称方法
        /// </summary>
        protected void SaveDisplayName(EditContext context)
        {
            if (!string.IsNullOrEmpty(User.UserName) && Bootstrap.DataAccess.UserHelper.SaveDisplayName(User.UserName, DisplayName))
            {
                User.DisplayName = DisplayName;
                RootLayout?.OnDisplayNameChanged(DisplayName);
            }
        }

        /// <summary>
        /// 保存密码方法
        /// </summary>
        protected void SavePassword(EditContext context)
        {
            Bootstrap.DataAccess.UserHelper.ChangePassword(User.UserName, Password.Password, Password.NewPassword);
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
