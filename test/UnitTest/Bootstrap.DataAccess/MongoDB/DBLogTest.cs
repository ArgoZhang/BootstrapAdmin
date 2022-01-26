// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Xunit;

namespace Bootstrap.DataAccess.MongoDB
{
    [Collection("MongoContext")]
    public class DBLogTest : DataAccess.DBLogTest
    {
        [Fact]
        public override void Save_Ok()
        {
            Assert.True(new DBLog().Save(null));
        }
    }
}
