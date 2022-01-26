// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.EFCore;
using BootstrapAdmin.DataAccess.EFCore.Services;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionConfigure"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddEFCoreDataAccessServices(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionConfigure, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.AddDbContextFactory<BootstrapAdminContext>(optionConfigure, lifetime);

        services.AddServices();
        return services;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionConfigure"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddEFCoreDataAccessServices(this IServiceCollection services, Action<DbContextOptionsBuilder> optionConfigure, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.AddDbContextFactory<BootstrapAdminContext>(optionConfigure, lifetime);

        services.AddServices();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));
        // 增加缓存服务
        services.AddCacheManager();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IUser, UserService>();
        services.AddSingleton<IRole, RoleService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<ILogin, LoginService>();
        return services;
       
    }
}
