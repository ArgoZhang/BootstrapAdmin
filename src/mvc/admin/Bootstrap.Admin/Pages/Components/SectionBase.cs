// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SectionBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowBackground { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int LockScreenPeriod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowCardTitle { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
