// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class SQLTest : ControllerTest
    {
        public SQLTest(BALoginWebHost factory) : base(factory, "api/SQL") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetFromJsonAsync<QueryData<DBLog>>("?userName=Admin&OperateTimeStart=&OperateTimeEnd=");
            Assert.NotNull(resp);
        }
    }
}
