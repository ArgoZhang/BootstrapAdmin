using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Net.Http;
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

            // receive log
            var recv = await Client.GetAsJsonAsync<bool>("/api/TasksLog?name=周期任务");
            Assert.True(recv);

            // for test SignalRManager.SendTaskLog
            await System.Threading.Tasks.Task.Delay(6000);
        }
    }
}
