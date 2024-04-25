// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.EFCore;
using BootstrapAdmin.DataAccess.EFCore.Services;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// EFCore ORM 注入服务扩展类
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// 注入 EFCore 数据服务类
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddEFCoreDataAccessServices(this IServiceCollection services)
    {
        // 增加缓存服务
        services.AddCacheManager();

        services.AddDbContextFactory<BootstrapAdminContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connString = configuration.GetConnectionString("ba");
            options.UseSqlite(connString);
#if DEBUG
            options.LogTo(System.Console.WriteLine);
#endif
        }, ServiceLifetime.Singleton);

        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IUser, UserService>();
        services.AddSingleton<IRole, RoleService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<ITrace, TraceService>();
        services.AddSingleton<IApp, AppService>();
        services.AddSingleton<IException, ExceptionService>();
        return services;
    }
}
