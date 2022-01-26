// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// PageContentAttributes 实体类
    /// </summary>
    public class PageContentAttributes
    {
        /// <summary>
        /// 获得/设置 页面 ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 页面名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 获得/设置 是否显示
        /// </summary>
        public bool Active { get; set; }
    }
}
