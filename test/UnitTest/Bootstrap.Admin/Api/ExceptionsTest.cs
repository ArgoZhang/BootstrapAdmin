using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using static Bootstrap.Admin.Controllers.Api.ExceptionsController;

namespace Bootstrap.Admin.Api
{
    public class ExceptionsTest : ControllerTest
    {
        public ExceptionsTest(BALoginWebHost factory) : base(factory, "api/Exceptions") { }

        [Fact]
        public async void Get_Ok()
        {
            // insert exception
            var excep = new Exceptions();
            Assert.True(excep.Log(new Exception("UnitTest"), null));

            // 菜单 系统菜单 系统使用条件
            var query = "?sort=LogTime&order=desc&offset=0&limit=20&StartTime=&EndTime=&_=1547610349796";
            var option = new JsonSerializerOptions().AddDefaultConverters();
            var qd = await Client.GetFromJsonAsync<QueryData<Exceptions>>(query, option);
            Assert.NotEmpty(qd.rows);

            // clean
            DbManager.Create().Execute("delete from Exceptions where AppDomainName = @0", AppDomain.CurrentDomain.FriendlyName);
        }

        [Fact]
        public async void Post_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string>(string.Empty, "");
            var files = await ret.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.NotNull(files);

            var fileName = files.FirstOrDefault();
            if (!string.IsNullOrEmpty(fileName))
            {
                ret = await Client.PutAsJsonAsync<ExceptionFileQuery>("", new ExceptionFileQuery() { FileName = fileName });
                var resp = await ret.Content.ReadAsStringAsync();
                Assert.NotNull(resp);
            }

            // clean
            DbManager.Create().Execute("delete from Exceptions where AppDomainName = @0", AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
