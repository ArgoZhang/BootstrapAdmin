using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class SQLTest : ControllerTest
    {
        public SQLTest(BALoginWebHost factory) : base(factory, "api/SQL") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<QueryData<DBLog>>("?userName=Admin&OperateTimeStart=&OperateTimeEnd=");
            Assert.NotNull(resp);
        }
    }
}
