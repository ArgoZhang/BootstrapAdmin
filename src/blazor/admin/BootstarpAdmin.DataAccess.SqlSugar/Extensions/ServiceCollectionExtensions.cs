// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.DataAccess.SqlSugar.Extensions;
using BootstrapAdmin.DataAccess.SqlSugar.Service;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// SqlSugar ORM 注入服务扩展类
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注入 SqlSugar 数据服务类
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddSqlSugarDataAccessServices(this IServiceCollection services)
    {
        // 增加缓存服务
        services.AddCacheManager();

        services.TryAddSingleton<ISqlSugarClient>(provider =>
        {
            StaticConfig.Check_StringIdentity = false;
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connString = configuration.GetConnectionString("ba");
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = SqlSugarHelper.InitConfigureExternalServices()

            }, db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    System.Console.WriteLine(sql);
                };
            });
        });

        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

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
