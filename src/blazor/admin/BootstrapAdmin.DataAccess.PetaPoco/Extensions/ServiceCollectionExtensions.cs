// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.PetaPoco.Services;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// PetaPoco ORM 扩展数据服务类
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 增加 PetaPoco 数据服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPetaPocoDataAccessServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IDBManager, DBManagerService>();

        // 增加数据服务（未复用 Blazor 扩展 PetaPoco 服务有一些特殊处理）
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

        // 增加缓存服务
        services.AddCacheManager();

        // 增加业务服务
        services.AddSingleton<IApp, AppService>();
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IException, ExceptionService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IRole, RoleService>();
        services.AddSingleton<IUser, UserService>();
        services.AddSingleton<ITrace, TraceService>();
        return services;
    }
}
