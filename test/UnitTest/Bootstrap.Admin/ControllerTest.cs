// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin
{
    [Collection("Login")]
    public class ControllerTest
    {
        protected HttpClient Client { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="baseAddress"></param>
        public ControllerTest(BALoginWebHost factory, string baseAddress = "api")
        {
            Client = factory.CreateClient(baseAddress);
        }
    }
}
