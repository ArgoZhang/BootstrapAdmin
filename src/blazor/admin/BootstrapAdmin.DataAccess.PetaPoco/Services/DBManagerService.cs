// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetaPoco;
using PetaPoco.Providers;
using System.Collections.Specialized;
using System.Data.Common;
using System.Text;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class DBManagerService(IConfiguration configuration, ILogger<DBManagerService> logger, IWebHostEnvironment host) : IDBManager
{
    /// <summary>
    /// 创建 IDatabase 实例方法
    /// </summary>
    /// <param name="connectionName">连接字符串键值</param>
    /// <param name="keepAlive"></param>
    /// <returns></returns>
    public IDatabase Create(string connectionName = "ba", bool keepAlive = false)
    {
        var conn = configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(nameof(connectionName));

        var option = DatabaseConfiguration.Build();
        option.UsingDefaultMapper<BootstrapAdminConventionMapper>();

        // connectionString
        option.UsingConnectionString(conn);

        // provider
        option.UsingProvider<SQLiteDatabaseProvider>();

        var db = new Database(option) { KeepConnectionAlive = keepAlive };

        db.ExceptionThrown += (sender, e) =>
        {
            var message = e.Exception.Format(new NameValueCollection()
            {
                [nameof(db.LastCommand)] = db.LastCommand,
                [nameof(db.LastArgs)] = string.Join(",", db.LastArgs)
            });
            logger.LogError(e.Exception, "{Message}", message);
        };
        if (host.IsDevelopment())
        {
            db.CommandExecuted += (sender, args) =>
            {
                var parameters = new StringBuilder();
                foreach (DbParameter p in args.Command.Parameters)
                {
                    parameters.AppendFormat("{0}: {1}  ", p.ParameterName, p.Value);
                }
                logger.LogInformation("{CommandText}", args.Command.CommandText);
                logger.LogInformation("{CommandArgs}", parameters.ToString());
            };
        };
        return db;
    }
}
