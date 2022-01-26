// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryData<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new T[0];

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public int PageItems { get; set; } = 20;
    }
}
