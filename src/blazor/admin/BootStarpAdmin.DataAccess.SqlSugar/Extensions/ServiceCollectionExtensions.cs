﻿using BootStarpAdmin.DataAccess.SqlSugar.Service;
using BootstrapBlazor.Components;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// FreeSql ORM 注入服务扩展类
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注入 FreeSql 数据服务类
    /// </summary>
    /// <param name="services"></param>
    /// <param name="sqlSugarBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlSugar(this IServiceCollection services, Action<IServiceProvider, ConnectionConfig> sqlSugarBuilder)
    {
        services.TryAddSingleton<ISqlSugarClient>(provider =>
        {
            var builder = new ConnectionConfig();
            builder.IsAutoCloseConnection = true;
            sqlSugarBuilder(provider, builder);
            return new SqlSugarClient(builder);
        });

        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

        // 增加业务服务
        //services.AddSingleton<IApp, AppService>();
        //services.AddSingleton<IDict, DictService>();
        //services.AddSingleton<IException, ExceptionService>();
        //services.AddSingleton<IGroup, GroupService>();
        //services.AddSingleton<ILogin, LoginService>();
        //services.AddSingleton<INavigation, NavigationService>();
        //services.AddSingleton<IRole, RoleService>();
        //services.AddSingleton<IUser, UserService>();
        return services;
    }
}
