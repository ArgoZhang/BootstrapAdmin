using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class ResetUserTest
    {
        [Fact]
        public void Save_Ok()
        {
            var resetUser = new ResetUser()
            {
                UserName = "UnitTest",
                Reason = "UnitTest",
                DisplayName = "UnitTest",
                ResetTime = DateTime.Now
            };
            var db = DbManager.Create();
            db.Save(resetUser);
            var count = db.ExecuteScalar<int>("select count(Id) from ResetUsers");
            Assert.True(count > 0);
        }

        [Fact]
        public void RetrieveUserByUserName_Ok()
        {
            var resetUser = new ResetUser()
            {
                UserName = "UnitTest",
                Reason = "UnitTest",
                DisplayName = "UnitTest",
                ResetTime = DateTime.Now
            };
            var db = DbManager.Create();
            db.Save(resetUser);

            var user = resetUser.RetrieveUserByUserName(resetUser.UserName);
            Assert.Equal("UnitTest", user.UserName);
            Assert.Equal("UnitTest", user.DisplayName);
        }

        [Fact]
        public void DeleteByUserName_Ok()
        {
            var resetUser = new ResetUser()
            {
                UserName = "UnitTest",
                Reason = "UnitTest",
                DisplayName = "UnitTest",
                ResetTime = DateTime.Now
            };
            var db = DbManager.Create();
            db.Save(resetUser);

            resetUser.DeleteByUserName(resetUser.UserName);
            var count = db.ExecuteScalar<int>("select count(Id) from ResetUsers");
            Assert.Equal(0, count);
        }
    }
}
