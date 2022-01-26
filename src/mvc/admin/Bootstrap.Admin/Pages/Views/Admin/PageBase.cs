// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 页面组件基类
    /// </summary>
    public abstract class PageBase : ComponentBase
    {
        /// <summary>
        /// 是否重新绘制组件方法
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldRender() => false;
    }
}
