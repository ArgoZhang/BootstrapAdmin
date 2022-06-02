// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapBlazor.Components;
using BootstrapClient.DataAccess.PetaPoco;
using BootstrapClient.DataAccess.PetaPoco.Services;
using BootstrapClient.Web.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System.Collections.Specialized;
using System.Data.Common;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPetaPocoDataAccessServices(this IServiceCollection services)
    {
        // 增加多数据库支持服务
        services.TryAddSingleton<IDBManager, DBManagerService>();

        // 增加业务服务
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IUser, UserService>();

        // 增加示例数据服务
        services.AddSingleton<IDummy, DummyService>();
        return services;
    }
}
