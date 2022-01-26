// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapClient.DataAccess.Models;

namespace BootstrapClient.Web.Core;

/// <summary>
/// 
/// </summary>
public interface INavigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Navigation> GetMenus(string userName);
}
