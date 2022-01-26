// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Xunit;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 正常系统
    /// </summary>
    [CollectionDefinition("Login")]
    public class BootstrapAdminContext : ICollectionFixture<BALoginWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BALoginWebHost : BAWebHost
    {
        /// <summary>
        /// 
        /// </summary>
        public BALoginWebHost()
        {
            var client = CreateClient("Account/Login");
            var login = client.LoginAsync();
            login.Wait();
        }
    }
}
