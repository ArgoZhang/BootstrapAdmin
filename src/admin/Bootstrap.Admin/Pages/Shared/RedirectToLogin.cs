using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Admin.Pages.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        [NotNull]
        private NavigationManager? Navigation { get; set; }

        /// <summary>
        /// OnAfterRender 方法
        /// </summary>
        protected override void OnAfterRender(bool firstRender)
        {
            Navigation.NavigateTo(CookieAuthenticationDefaults.LoginPath, true);
        }
    }
}
