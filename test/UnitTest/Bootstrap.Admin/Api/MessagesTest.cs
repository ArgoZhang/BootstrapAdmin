using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class MessagesTest : ControllerTest
    {
        public MessagesTest(BALoginWebHost factory) : base(factory, "api/Messages") { }

        [Theory]
        [InlineData("inbox")]
        [InlineData("sendmail")]
        [InlineData("mark")]
        [InlineData("trash")]
        public async void Get_Ok(string action)
        {
            var resp = await Client.GetFromJsonAsync<IEnumerable<Message>>(action);
            Assert.NotNull(resp);
        }

        [Fact]
        public async void GetCount_Ok()
        {
            var resp = await Client.GetFromJsonAsync<MessageCountModel>("");
            Assert.NotNull(resp);
        }
    }
}
