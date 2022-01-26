// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// IRules 接口
    /// </summary>
    public interface IRules
    {
        /// <summary>
        /// 获得 Rules 集合
        /// </summary>
        ICollection<ValidatorComponentBase> Rules { get; }
    }
}
