// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.Data;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [CollectionDefinition("SQLServerContext")]
    public class BootstrapAdminTestContext : ICollectionFixture<BASQLServerWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BASQLServerWebHost : BALoginWebHost
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureDatabase(DatabaseProviderType.SqlServer);
        }
    }
}
