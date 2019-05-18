using System;
using Xunit;

namespace Bootstrap.DataAccess.SqlServer
{
    [Collection("SQLServerContext")]
    public class MessagesTest
    {
        [Fact]
        public void RetrieveHeaders_Ok()
        {
            Assert.NotNull(MessageHelper.Retrieves("Admin"));
        }

        [Fact]
        public virtual void Save_Ok()
        {
            var msg = new Message()
            {
                Content = "UnitTest",
                From = "Admin",
                Label = "Test",
                IsDelete = 0,
                Flag = 0,
                Period = "1",
                SendTime = DateTime.Now,
                Status = "0",
                Title = "Test",
                To = "User",
                LabelName = "UnitTest",
                FromDisplayName = "UnitTest",
                FromIcon = "Default.jpg"
            };
            Assert.True(MessageHelper.Save(msg));


        }
    }
}
