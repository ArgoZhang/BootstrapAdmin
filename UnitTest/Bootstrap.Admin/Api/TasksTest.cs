using Bootstrap.DataAccess;
using System.Collections.Generic;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class TasksTest : ControllerTest
    {
        public TasksTest(BAWebHost factory) : base(factory, "api/Tasks") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<Task>>();
            Assert.NotNull(resp);
        }
    }
}
