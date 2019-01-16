using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class LogsTest : ApiWebHost
    {
        public LogsTest(BAWebHost factory) : base(factory, "Logs", true)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=LogTime&order=desc&offset=0&limit=20&operateType=&OperateTimeStart=&OperateTimeEnd=&_=1547617573596";
            var qd = await Client.GetAsJsonAsync<QueryData<Log>>(query);
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void Post_Ok()
        {
            Client.DefaultRequestHeaders.Add("user-agent", "UnitTest");
            var resp = await Client.PostAsJsonAsync<Log, bool>("", new Log() { CRUD = "UnitTest", RequestUrl = "~/UnitTest" });
            Assert.True(resp);
        }
    }
}
