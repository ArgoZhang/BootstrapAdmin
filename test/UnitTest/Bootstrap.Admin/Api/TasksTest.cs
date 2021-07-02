using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class TasksTest : ControllerTest
    {
        public TasksTest(BALoginWebHost factory) : base(factory, "api/Tasks") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetFromJsonAsync<IEnumerable<Task>>("");
            Assert.NotNull(resp);

            // receive log
            var recv = await Client.GetFromJsonAsync<bool>("/api/TasksLog?name=周期任务");
            Assert.True(recv);

            // for test SignalRManager.SendTaskLog
            await System.Threading.Tasks.Task.Delay(6000);
        }

        [Fact]
        public async void Put_Ok()
        {
            var resp = await Client.PutAsJsonAsync<string>("/api/Tasks/SQL日志?operType=pause", "");
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            // receive log
            var recv = await Client.PutAsJsonAsync<string>("/api/Tasks/SQL日志?operType=run", "");
            ret = await recv.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }

        [Fact]
        public async void Post_Ok()
        {
            var widget = new TaskWidget();

            // widget Cron 表达式为 ”“
            var resp = await Client.PostAsJsonAsync<TaskWidget>("/api/Tasks", widget);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.False(ret);

            // task executor 不合法
            widget.CronExpression = Longbow.Tasks.Cron.Secondly(5);
            widget.TaskExecutorName = "UnitTest-Widget";
            resp = await Client.PostAsJsonAsync<TaskWidget>("/api/Tasks", widget);
            Assert.False(resp.IsSuccessStatusCode);

            widget.TaskExecutorName = "Bootstrap.Admin.DefaultTaskExecutor";
            widget.Name = "UnitTest-Task";
            resp = await Client.PostAsJsonAsync<TaskWidget>("/api/Tasks", widget);
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            // Delete
            resp = await Client.DeleteAsJsonAsync<IEnumerable<string>>("/api/Tasks", new string[] { widget.Name });
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }

        [Fact]
        public void DefaultTaskExecutor_Ok()
        {
            var executor = new DefaultTaskExecutor();
            executor.Execute(new System.Threading.CancellationToken());
        }
    }

    [Collection("SystemModel")]
    public class TasksSystemModelTest
    {
        private readonly HttpClient client;

        public TasksSystemModelTest(BASystemModelWebHost factory)
        {
            client = factory.CreateClient("/api/Tasks");
        }

        [Fact]
        public async void Post_Ok()
        {
            var widget = new TaskWidget
            {
                CronExpression = Longbow.Tasks.Cron.Secondly(5),
                Name = "单次任务",
                TaskExecutorName = "Bootstrap.Admin.DefaultTaskExecutor"
            };

            // 演示模式下禁止移除系统内置任务
            var resp = await client.PostAsJsonAsync<TaskWidget>("/api/Tasks", widget);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.False(ret);

            resp = await client.DeleteAsJsonAsync<IEnumerable<string>>("/api/Tasks", new string[] { widget.Name });
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.False(ret);

            widget.Name = "Test-Widget";
            resp = await client.PostAsJsonAsync<TaskWidget>("/api/Tasks", widget);
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            resp = await client.DeleteAsJsonAsync<IEnumerable<string>>("/api/Tasks", new string[] { widget.Name });
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }
    }
}
