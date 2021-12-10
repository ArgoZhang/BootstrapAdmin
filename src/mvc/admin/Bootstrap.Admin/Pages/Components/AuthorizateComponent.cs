using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.Security.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 授权组件
    /// </summary>
    public class AuthorizateComponent : ComponentBase
    {
        /// <summary>
        /// 授权键值
        /// </summary>
        [Parameter]
        public string Key { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IButtonAuthorization? AuthorizationServices { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Inject]
        protected AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        private bool authorizated;

        /// <summary>
        /// 
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            if (AuthenticationStateProvider != null)
            {
                var user = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var url = new UriBuilder(NavigationManager?.Uri ?? "").Path;
                authorizated = AuthorizationServices != null && AuthorizationServices.Authorizate(user.User, url.ToMvcMenuUrl(), Key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (authorizated) builder.AddContent(0, ChildContent);
        }
    }
}
