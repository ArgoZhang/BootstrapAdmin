// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Components.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Logins
{
    /// <summary>
    /// 
    /// </summary>
    public ITableSearchModel TableSearchModel { get; } = new LoginLogModel();
}
