// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetaPoco;
using PetaPoco.Providers;
using System.Collections.Specialized;

namespace BootstrapClient.DataAccess.PetaPoco.Services;

internal class DBManagerService
{
    private IConfiguration Configuration { get; set; }

    private ILogger<DBManagerService> Logger { get; set; }

    public DBManagerService(IConfiguration configuration, ILogger<DBManagerService> logger)
    {
        Configuration = configuration;
        Logger = logger;
    }

    /// <summary>
    /// 创建 IDatabase 实例方法
    /// </summary>
    /// <param name="connectionName">连接字符串键值</param>
    /// <param name="keepAlive"></param>
    /// <returns></returns>
    public IDatabase Create(string? connectionName = "client", bool keepAlive = false)
    {
        var conn = Configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(nameof(connectionName));

        var source = DatabaseConfiguration.Build();

        // connectionstring
        source.UsingConnectionString(conn);

        // provider
        source.UsingProvider<SqlServerDatabaseProvider>();
        var db = new Database(source) { KeepConnectionAlive = keepAlive };
        db.ExceptionThrown += (sender, args) =>
        {
            Logger.LogError(args.Exception, $"Last-Cmd: {db.LastCommand}");
        };
        return db;
    }
}
