// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class GiteeTest : ControllerTest
    {
        public GiteeTest(BALoginWebHost factory) : base(factory, "api/Gitee") { }

        [Fact]
        public async void Issues_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Issues");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Pulls_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Pulls");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Releases_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Releases");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Builds_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Builds");
            Assert.NotNull(cates);
        }
    }
}
