using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Client.Blazor.Shared.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        [NotNull]
        private IConfiguration? Configuration { get; set; }

        [Inject]
        [NotNull]
        private NavigationManager? Navigator { get; set; }

#if DEBUG
        /// <summary>
        /// OnAfterRender 方法
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            var option = Configuration.GetBootstrapAdminAuthenticationOptions();
            Navigator.NavigateTo(option.AuthHost, true);
        }
#else
        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            var option = Configuration.GetBootstrapAdminAuthenticationOptions();
            Navigator.NavigateTo(option.AuthHost, true);
        }
#endif
    }
}
