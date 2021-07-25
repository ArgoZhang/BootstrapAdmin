using Bootstrap.DataAccess;
using Longbow.Web;
using Longbow.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class TracesTest : ControllerTest
    {
        public TracesTest(BALoginWebHost factory) : base(factory, "api/Traces") { }

        [Fact]
        public async void Get_Ok()
        {
            var trac = new Trace() { Browser = "UnitTest", OS = "UnitTest", City = "本地连接", Ip = "::1", RequestUrl = "~/UnitTest", UserName = "UnitTest", LogTime = DateTime.Now };
            trac.Save(trac);

            // 菜单 系统菜单 系统使用条件
            var query = "?sort=LogTime&order=desc&offset=0&limit=20&operateType=&OperateTimeStart=&OperateTimeEnd=&AccessIP=&_=1547617573596";
            var qd = await Client.GetFromJsonAsync<QueryData<Trace>>(query, new System.Text.Json.JsonSerializerOptions().AddDefaultConverters());
            Assert.NotEmpty(qd.rows);

            // clean
            DbManager.Create().Execute("Delete from Traces where LogTime = @0", trac.LogTime);
        }

        [Fact]
        public async void Post_Ok()
        {
            var onlineUser = new OnlineUser()
            {
                Ip = "UniTest",
                RequestUrl = "UniTest",
                LastAccessTime = DateTime.Now,
                Location = "UniTest",
                Browser = "UniTest",
                OS = "UniTest",
                UserAgent = "UniTest"
            };
            var result = await Client.PostAsJsonAsync<OnlineUser>("", onlineUser);
            var ret = await result.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }
    }
}
