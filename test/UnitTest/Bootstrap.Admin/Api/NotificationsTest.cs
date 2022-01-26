// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class NotificationsTest : ControllerTest
    {
        public NotificationsTest(BALoginWebHost factory) : base(factory, "api/Notifications") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetFromJsonAsync<object>("");
            Assert.NotNull(resp);
        }
    }
}
