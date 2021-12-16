using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Pages.Account
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Login
    {
        [Inject]
        [NotNull]
        private IDicts? DictsService { get; set; }

        private string? Title { get; set; }

        private bool AllowMobile { get; set; }

        private bool AllowOAuth { get; set; } = true;

        [NotNull]
        private string? UserName { get; set; }

        [NotNull]
        private string? Password { get; set; }

        private bool RememberPassword { get; set; }

        [Inject]
        [NotNull]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        [NotNull]
        private LoginService? LoginService { get; set; }

        [Inject]
        [NotNull]
        private IUsers? UserService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Title = DictsService.GetWebTitle();
        }

        void OnClickMobile()
        {
            AllowMobile = true;
        }

        void OnSignIn()
        {
            var auth = UserService.Authenticate(UserName, Password);

            if (auth)
            {
                LoginService.LoginSeessionId = Guid.NewGuid().ToString();
                LoginService.UserName = UserName;
                LoginService.Remember = RememberPassword;
                Navigation.NavigateTo($"/Login?id={LoginService.LoginSeessionId}", true);
            }
        }

        void OnSignUp()
        {

        }

        void OnForgotPassword()
        {

        }
    }
}
