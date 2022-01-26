// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DictSearch
    {
        private IEnumerable<SelectedItem>? Items { get; set; } = typeof(EnumDictDefine).ToSelectList(new SelectedItem("", "全部"));

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public DictsSearchModel? Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<DictsSearchModel> ValueChanged { get; set; }
    }
}
