// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace Bootstrap.Admin
{
    [CollectionDefinition("MongoContext")]
    public class MongoContext : ICollectionFixture<BAMongoDBWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BAMongoDBWebHost : BALoginWebHost
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                new KeyValuePair<string, string>("DB:1:Enabled", "false"),
                new KeyValuePair<string, string>("DB:2:Enabled", "false"),
                new KeyValuePair<string, string>("DB:3:Enabled", "false"),
                new KeyValuePair<string, string>("DB:4:Enabled", "true")
            }));
        }
    }
}
