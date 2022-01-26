// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.Components;
using System;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectItemBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public SelectedItem Item { get; set; } = new SelectedItem();

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Action<SelectedItem> ItemClickCallback { get; set; } = new Action<SelectedItem>(SelectedItem => { });
    }
}
