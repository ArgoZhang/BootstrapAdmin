using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Bootstrap.Admin.Controllers.Api.ExceptionsController;

namespace Bootstrap.Admin.Api
{
    public class ExceptionsTest : ApiTest
    {
        public ExceptionsTest(BAWebHost factory) : base(factory, "Exceptions", true)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=LogTime&order=desc&offset=0&limit=20&StartTime=&EndTime=&_=1547610349796";
            var qd = await Client.GetAsJsonAsync<QueryData<BootstrapDict>>(query);
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void Post_Ok()
        {
            var files = await Client.PostAsJsonAsync<string, IEnumerable<string>>("", string.Empty);
            Assert.NotNull(files);

            var fileName = files.FirstOrDefault();
            if (!string.IsNullOrEmpty(fileName))
            {
                var resp = await Client.PutAsJsonAsync<ExceptionFileQuery, string>("", new ExceptionFileQuery() { FileName = fileName });
                Assert.NotNull(resp);
            }
        }
    }
}
