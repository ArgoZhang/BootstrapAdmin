// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapClient.DataAccess.Models;
using BootstrapClient.Web.Core;
using PetaPoco;

namespace BootstrapClient.DataAccess.PetaPoco.Services;

internal class DummyService : IDummy
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public DummyService(IDBManager db)
    {
        DBManager = db;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<DummyEntity> GetAll()
    {
        return new List<DummyEntity>()
        {
            new() { Id= "1", Name ="Dummy1" },
            new() { Id= "2", Name ="Dummy2" }
        };
    }
}
