using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Collections.Generic;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class MessagesTest : ApiWebHost
    {
        public MessagesTest(BAWebHost factory) : base(factory, "Messages", true)
        {

        }

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
            var resp = await Client.GetAsJsonAsync<MessageCountModel>(string.Empty);
            Assert.NotNull(resp);
        }
    }
}
