using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class MessagesTest : ControllerTest
    {
        public MessagesTest(BAWebHost factory) : base(factory, "api/Messages") { }

        [Theory]
        [InlineData("inbox")]
        [InlineData("sendmail")]
        [InlineData("mark")]
        [InlineData("trash")]
        public async void Get_Ok(string action)
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<Message>>(action);
            Assert.NotNull(resp);
        }

        [Fact]
        public async void GetCount_Ok()
        {
            var resp = await Client.GetAsJsonAsync<MessageCountModel>();
            Assert.NotNull(resp);
        }
    }
}
