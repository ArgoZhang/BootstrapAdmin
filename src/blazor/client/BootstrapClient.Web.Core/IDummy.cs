// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapClient.DataAccess.Models;

namespace BootstrapClient.Web.Core;

/// <summary>
/// 数据服务示例接口
/// </summary>
public interface IDummy
{
    /// <summary>
    /// 获得 全部数据
    /// </summary>
    /// <returns></returns>
    List<DummyEntity> GetAll();
}
