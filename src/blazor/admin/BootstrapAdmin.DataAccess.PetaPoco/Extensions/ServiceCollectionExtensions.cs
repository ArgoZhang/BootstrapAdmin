// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.PetaPoco;
using BootstrapAdmin.DataAccess.PetaPoco.Services;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
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
        services.TryAddSingleton<IDBManager, DBManagerService>();

        // 增加数据服务
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
