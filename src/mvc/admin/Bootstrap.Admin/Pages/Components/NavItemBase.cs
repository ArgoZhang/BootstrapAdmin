// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Security;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class NavItemBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public BootstrapMenu Menu { get; set; } = new BootstrapMenu();
    }
}
