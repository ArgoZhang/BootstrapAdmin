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
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IServiceCollection AddPetaPocoDataAccessServices(this IServiceCollection services, Action<IServiceProvider, IDatabaseBuildConfiguration> builder)
    {
        services.TryAddSingleton<IDatabase>(provider =>
        {
            var option = DatabaseConfiguration.Build();
            builder(provider, option);
            option.UsingDefaultMapper<BootstrapAdminConventionMapper>();
            var db = new Database(option);

            var logger = provider.GetRequiredService<ILogger<Database>>();
            db.ExceptionThrown += (sender, e) =>
            {
                var message = e.Exception.Format(new NameValueCollection()
                {
                    [nameof(db.LastCommand)] = db.LastCommand,
                    [nameof(db.LastArgs)] = string.Join(",", db.LastArgs)
                });
                logger.LogError(new EventId(1001, "GlobalException"), e.Exception, message);
            };
            var env = provider.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                db.CommandExecuted += (sender, args) =>
                {
                    var parameters = new StringBuilder();
                    foreach (DbParameter p in args.Command.Parameters)
                    {
                        parameters.AppendFormat("{0}: {1}  ", p.ParameterName, p.Value);
                    }
                    logger.LogInformation(args.Command.CommandText);
                    logger.LogInformation(parameters.ToString());
                };
            };
            return db;
        });

        // 增加多数据库支持服务
        services.AddSingleton<DBManagerService>();

        // 增加业务服务
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IUser, UserService>();

        // 增加示例数据服务
        services.AddSingleton<IDummy, DummyService>();
        return services;
    }
}
