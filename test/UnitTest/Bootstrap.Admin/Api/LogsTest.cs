using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class LogsTest : ControllerTest
    {
        public LogsTest(BALoginWebHost factory) : base(factory, "api/Logs") { }

        [Fact]
        public async void Get_Ok()
        {
            var log = new Log() { CRUD = "UnitTest", Browser = "UnitTest", OS = "UnitTest", City = "本地连接", Ip = "::1", RequestUrl = "~/UnitTest", UserName = "UnitTest", LogTime = DateTime.Now };
            log.Save(log);

            // 菜单 系统菜单 系统使用条件
            var query = "?sort=LogTime&order=desc&offset=0&limit=20&operateType=&OperateTimeStart=&OperateTimeEnd=&_=1547617573596";
            var qd = await Client.GetFromJsonAsync<QueryData<Log>>(query, new System.Text.Json.JsonSerializerOptions().AddDefaultConverters());
            Assert.NotEmpty(qd.rows);

            // clean
            DbManager.Create().Execute("Delete from Logs where CRUD = @0", log.CRUD);
        }

        [Fact]
        public async void Post_Ok()
        {
            Client.DefaultRequestHeaders.Add("user-agent", "UnitTest");
            var resp = await Client.PostAsJsonAsync<Log>("", new Log() { CRUD = "UnitTest", RequestUrl = "~/UnitTest" });
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            // clean
            DbManager.Create().Execute("delete from Logs where CRUD = @0", "UnitTest");
        }
    }
}
