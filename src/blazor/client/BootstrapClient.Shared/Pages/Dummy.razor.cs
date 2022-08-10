// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapClient.DataAccess.Models;
using BootstrapClient.Web.Core;

namespace BootstrapClient.Web.Shared.Pages;

/// <summary>
/// 示例数据服务
/// </summary>
public partial class Dummy
{
    [Inject]
    [NotNull]
    private IDummy? DummyService { get; set; }

    private Task<QueryData<DummyEntity>> OnQueryAsync(QueryPageOptions options)
    {
        var items = DummyService.GetAll();
        var ret = new QueryData<DummyEntity>()
        {
            Items = items
        };
        return Task.FromResult(ret);
    }
}
