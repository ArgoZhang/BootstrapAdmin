using System;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class ResetUserTest
    {
        [Fact]
        public void ResetReasonsByUserName_Ok()
        {
            var user = new User { UserName = "UnitTestReset", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            UserHelper.Delete(UserHelper.Retrieves().Union(UserHelper.RetrieveNewUsers()).Where(u => u.UserName == user.UserName).Select(u => u.Id));
            Assert.True(UserHelper.Save(user));

            UserHelper.ForgotPassword(new ResetUser() { UserName = user.UserName, DisplayName = user.DisplayName, Reason = "UnitTest", ResetTime = DateTime.Now });
            Assert.NotNull(UserHelper.RetrieveResetUserByUserName(user.UserName));

            var reasons = UserHelper.RetrieveResetReasonsByUserName(user.UserName);
            Assert.NotEmpty(reasons);
        }
    }
}
