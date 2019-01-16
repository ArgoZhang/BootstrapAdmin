using Bootstrap.DataAccess;
using System.Collections.Generic;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class TasksTest : ApiTest
    {
        public TasksTest(BAWebHost factory) : base(factory, "Tasks", true)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<Task>>();
            Assert.NotNull(resp);
        }
    }
}
