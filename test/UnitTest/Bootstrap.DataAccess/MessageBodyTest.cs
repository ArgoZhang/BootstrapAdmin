// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class MessageBodyTest
    {
        [Fact]
        public void ToString_Ok()
        {
            var body = new MessageBody();
            body.Category = "UnitTest";
            body.Message = "UnitTest";
            Assert.Equal($"{body.Category}-{body.Message}", body.ToString());
        }
    }
}
