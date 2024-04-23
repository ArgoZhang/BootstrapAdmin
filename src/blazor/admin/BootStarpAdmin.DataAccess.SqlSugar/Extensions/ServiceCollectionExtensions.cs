// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


using BootstrapAdmin.DataAccess.Models;
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
    /// <param name="dbType">数据库类型</param>
    /// <param name="dbName"></param>
    /// <param name="dbProvider"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlSugar(this IServiceCollection services, DbType dbType, string dbName, Action<IServiceProvider, SqlSugarClient>? dbProvider = null)
    {
        // 增加缓存服务
        services.AddCacheManager();
        services.TryAddSingleton<ISqlSugarClient>(provider =>
        {
            StaticConfig.Check_StringIdentity = false;
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connString = configuration.GetConnectionString(dbName);
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connString,
                DbType = dbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,

                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    EntityNameService = (type, entity) =>
                    {
                        if (entity.DbTableName == nameof(User))
                        {
                            entity.DbTableName = "Users";
                        }
                        else if (entity.DbTableName == nameof(Dict))
                        {
                            entity.DbTableName = "Dicts";
                        }
                        else if (entity.DbTableName == nameof(Navigation))
                        {
                            entity.DbTableName = "Navigations";
                        }
                        else if (entity.DbTableName == nameof(Group))
                        {
                            entity.DbTableName = "Groups";
                        }
                        else if (entity.DbTableName == nameof(Role))
                        {
                            entity.DbTableName = "Roles";
                        }
                        else if (entity.DbTableName == nameof(Error))
                        {
                            entity.DbTableName = "Errors";
                        }
                        else if (entity.DbTableName == nameof(Trace))
                        {
                            entity.DbTableName = "Traces";
                        }
                        else if (entity.DbTableName == nameof(LoginLog))
                        {
                            entity.DbTableName = "LoginLogs";
                        }
                    },
                    EntityService = (type, column) =>
                    {
                        if (column.DbTableName == "Users")
                        {
                            if (column.DbColumnName == nameof(User.Period)
                            || column.DbColumnName == nameof(User.ConfirmPassword)
                            || column.DbColumnName == nameof(User.NewPassword)
                            || column.DbColumnName == nameof(User.IsReset)
                            )
                            {
                                column.IsIgnore = true;
                            }
                        }
                        else if (column.DbTableName == "Navigations")
                        {
                            if (column.DbColumnName == nameof(Navigation.HasChildren))
                            {
                                column.IsIgnore = true;
                            }
                        }
                        if (column.DbColumnName.ToUpper() == "ID")
                        {
                            column.IsPrimarykey = true;
                            column.IsIdentity = true;
                        }
                    }
                }
            }, db =>
            {
                dbProvider?.Invoke(provider, db);
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    System.Console.WriteLine(sql);//输出sql
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
