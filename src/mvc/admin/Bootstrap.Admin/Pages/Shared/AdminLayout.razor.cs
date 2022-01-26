// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Admin.Pages.Components;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Shared
{
    /// <summary>
    /// AdminLayout 布局类
    /// </summary>
    public partial class AdminLayout
    {
        /// <summary>
        /// 
        /// </summary>
        public TabSet? TabSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout RootLayout { get; protected set; } = new DefaultLayout();
    }
}
