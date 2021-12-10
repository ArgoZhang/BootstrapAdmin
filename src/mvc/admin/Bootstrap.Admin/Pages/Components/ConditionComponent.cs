using System;
using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.Admin.Pages.Shared;
using Bootstrap.Security.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 条件输出组件
    /// </summary>
    public class ConditionComponent : ComponentBase
    {
        /// <summary>
        /// 获得/设置 IButtonAuthoriazation 实例
        /// </summary>
        [Inject]
        protected IButtonAuthorization? ComponentAuthorization { get; set; }

        /// <summary>
        /// 获得/设置 是否显示 默认 true 显示
        /// </summary>
        [Parameter]
        public bool Inverse { get; set; }

        /// <summary>
        /// 获得/设置 授权码
        /// </summary>
        [Parameter]
        public string AuthKey { get; set; } = "";

        /// <summary>
        /// 获得/设置 是否显示
        /// </summary>
        [Parameter]
        public bool? Condition { get; set; }

        /// <summary>
        /// 获得/设置 子控件
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 获得/设置 默认母版页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout? RootLayout { get; protected set; }

        /// <summary>
        /// 渲染组件方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // 授权码赋值时使用 IButtonAuthorization 服务进行判断
            var render = false;
            if (!string.IsNullOrEmpty(AuthKey))
            {
                var task = RootLayout?.AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (task != null)
                {
                    task.Wait();
                    var user = task.Result.User;
                    var url = new UriBuilder(RootLayout?.NavigationManager?.Uri ?? "").Path;
                    render = ComponentAuthorization?.Authorizate(user, url.ToMvcMenuUrl(), AuthKey) ?? false;
                }
            }
            else if (Condition.HasValue) render = Condition.Value;
            else render = RootLayout?.Model.IsDemo ?? false;
            if (Inverse) render = !render;
            if (render) builder.AddContent(0, ChildContent);
        }
    }
}
