// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

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

internal class DBManagerService : IDBManager
{
    private IConfiguration Configuration { get; set; }

    private ILogger<DBManagerService> Logger { get; set; }

    private IWebHostEnvironment WebHost { get; set; }

    public DBManagerService(IConfiguration configuration, ILogger<DBManagerService> logger, IWebHostEnvironment host)
    {
        Configuration = configuration;
        Logger = logger;
        WebHost = host;
    }

    /// <summary>
    /// 创建 IDatabase 实例方法
    /// </summary>
    /// <param name="connectionName">连接字符串键值</param>
    /// <param name="keepAlive"></param>
    /// <returns></returns>
    public IDatabase Create(string connectionName = "ba", bool keepAlive = false)
    {
        var conn = Configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(nameof(connectionName));

        var option = DatabaseConfiguration.Build();
        option.UsingDefaultMapper<BootstrapAdminConventionMapper>();

        // connectionstring
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
            Logger.LogError(e.Exception, "{Message}", message);
        };
        if (WebHost.IsDevelopment())
        {
            db.CommandExecuted += (sender, args) =>
            {
                var parameters = new StringBuilder();
                foreach (DbParameter p in args.Command.Parameters)
                {
                    parameters.AppendFormat("{0}: {1}  ", p.ParameterName, p.Value);
                }
                Logger.LogInformation("{CommandText}", args.Command.CommandText);
                Logger.LogInformation("{CommandArgs}", parameters.ToString());
            };
        };
        return db;
    }
}
