// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

/// <summary>
/// 
/// </summary>
public interface IDBManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IDatabase Create(string connectionName = "ba", bool keepAlive = false);
}
