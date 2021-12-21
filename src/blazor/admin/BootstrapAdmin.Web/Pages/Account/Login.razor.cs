using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Pages.Account
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Login
    {
        private string? Title { get; set; }

        private bool AllowMobile { get; set; } = true;

        private bool UseMobileLogin { get; set; }

        private bool AllowOAuth { get; set; } = true;

        private bool RememberPassword { get; set; }

        private string? PostUrl { get; set; } = "/Account/Login";

        [Inject]
        [NotNull]
        private IDict? DictsService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Title = DictsService.GetWebTitle();
        }

        void OnClickSwitchButton()
        {
            PostUrl = UseMobileLogin ? "/Account/Mobile" : "/Account/Login";
        }

        void OnSignUp()
        {

        }

        void OnForgotPassword()
        {

        }
    }
}
