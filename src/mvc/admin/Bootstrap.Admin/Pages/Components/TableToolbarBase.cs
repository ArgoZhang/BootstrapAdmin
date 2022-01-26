// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// Table Toolbar 组件
    /// </summary>
    public class TableToolbarBase : ComponentBase
    {
        /// <summary>
        /// Specifies the content to be rendered inside this
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 添加按钮到工具栏方法
        /// </summary>
        public void AddButtons(TableToolbarButton button) => Buttons.Add(button);

        /// <summary>
        /// 获得 Toolbar 按钮集合
        /// </summary>
        public ICollection<TableToolbarButton> Buttons { get; } = new HashSet<TableToolbarButton>();
    }
}
