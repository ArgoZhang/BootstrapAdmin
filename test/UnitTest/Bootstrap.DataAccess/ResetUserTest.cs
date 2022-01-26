// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class ResetUserTest
    {
        [Fact]
        public void ResetReasonsByUserName_Ok()
        {
            var user = new User { UserName = "UnitTestReset", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            Assert.True(UserHelper.Save(user));

            UserHelper.ForgotPassword(new ResetUser() { UserName = user.UserName, DisplayName = user.DisplayName, Reason = "UnitTest", ResetTime = DateTime.Now });
            Assert.NotNull(UserHelper.RetrieveResetUserByUserName(user.UserName));

            var reasons = UserHelper.RetrieveResetReasonsByUserName(user.UserName);
            Assert.NotEmpty(reasons);

            UserHelper.Delete(new string[] { user.Id });
        }

        [Fact]
        public void Save_Ok()
        {
            Assert.True(ResetUserHelper.Save(new ResetUser() { UserName = "U_Reset", DisplayName = "UnitTest", Reason = "UnitTest" }));
        }
    }
}
