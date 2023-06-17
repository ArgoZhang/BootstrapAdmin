// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Security.Blazor;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AdminCard
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? AuthorizeKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [EditorRequired]
        [NotNull]
        public string? HeaderText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Inject]
        [NotNull]
        private IBootstrapAdminService? AdminService { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? AppContext { get; set; }

        [Inject]
        [NotNull]
        private NavigationManager? NavigationManager { get; set; }

        private Task<bool> OnQueryCondition(string name)
        {
            var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            return Task.FromResult(AdminService.AuthorizingBlock(AppContext.UserName, url, name));
        }
    }
}
