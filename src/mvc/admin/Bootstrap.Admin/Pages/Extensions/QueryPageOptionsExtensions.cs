// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Admin.Pages.Components;
using Longbow.Web.Mvc;

namespace Bootstrap.Admin.Pages.Extensions
{
    /// <summary>
    /// QueryPageOptions 扩展操作类
    /// </summary>
    public static class QueryPageOptionsExtensions
    {
        /// <summary>
        /// QueryPageOptions 转换为 PaginationOption 方法
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static PaginationOption ToPaginationOption(this QueryPageOptions options)
        {
            return new PaginationOption()
            {
                Limit = options.PageItems,
                Offset = (options.PageIndex - 1) * options.PageItems,
                Order = options.SortOrder == SortOrder.Unset ? "" : options.SortOrder.ToString(),
                Sort = options.SortName,
                Search = options.SearchText
            };
        }
    }
}
