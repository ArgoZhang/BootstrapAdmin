// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.Web.Mvc;
using Xunit;
using System;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class LogsTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            var log = new Log()
            {
                UserName = "UnitTest",
                Browser = "UnitTest",
                City = "本地连接",
                OS = "UnitTest",
                Ip = "::1",
                CRUD = "UnitTest",
                RequestUrl = "~/Home/Index"
            };
            Assert.True(LogHelper.Save(log));
            Assert.NotNull(LogHelper.RetrievePages(new PaginationOption() { Limit = 20, Sort = "LogTime", Order = "desc" }, null, null, null));
            Assert.NotNull(LogHelper.RetrievePages(new PaginationOption() { Limit = 20, Sort = "CRUD", Order = "desc" }, null, null, null));
            Assert.NotNull(LogHelper.RetrievePages(new PaginationOption() { Limit = 20, Sort = "UserName", Order = "desc" }, null, null, null));
            Assert.NotNull(LogHelper.RetrievePages(new PaginationOption() { Limit = 20, Sort = "Ip", Order = "desc" }, null, null, null));
            Assert.NotNull(LogHelper.RetrievePages(new PaginationOption() { Limit = 20, Sort = "RequestUrl", Order = "desc" }, null, null, null));
            Assert.NotNull(LogHelper.RetrievePages(new PaginationOption() { Limit = 20, Sort = "RequestUrl", Order = "desc" }, DateTime.Now.AddDays(-1), DateTime.Now, "UnitTest"));
            Assert.NotEmpty(LogHelper.RetrieveAll(DateTime.Now.AddDays(-1), DateTime.Now, "UnitTest"));
        }
    }
}
