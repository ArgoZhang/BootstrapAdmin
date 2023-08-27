// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.FreeSql.Extensions;
using BootstrapAdmin.DataAccess.FreeSql.Service;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using FreeSql;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    /// <param name="freeSqlBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddFreeSql(this IServiceCollection services, Action<IServiceProvider, FreeSqlBuilder> freeSqlBuilder)
    {
        // 增加缓存服务
        services.AddCacheManager();
        services.TryAddSingleton<IFreeSql>(provider =>
        {
            var builder = new FreeSqlBuilder();
            freeSqlBuilder(provider, builder);
            var instance = builder.Build();
            instance.Mapper();
            return instance;
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
