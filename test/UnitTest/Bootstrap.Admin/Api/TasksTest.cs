using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class TasksTest : ControllerTest
    {
        public TasksTest(BALoginWebHost factory) : base(factory, "api/Tasks") { }

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

        [Fact]
        public async void Put_Ok()
        {
            var resp = await Client.PutAsJsonAsync<string, bool>("/api/Tasks/SQL日志?operType=pause", "");
            Assert.True(resp);

            // receive log
            var recv = await Client.PutAsJsonAsync<string, bool>("/api/Tasks/SQL日志?operType=run", "");
            Assert.True(recv);
        }

        [Fact]
        public async void Post_Ok()
        {
            var widget = new TaskWidget();

            // widget Cron 表达式为 ”“
            var resp = await Client.PostAsJsonAsync<TaskWidget, bool>("/api/Tasks", widget);
            Assert.False(resp);

            // task executor 不合法
            widget.CronExpression = Longbow.Tasks.Cron.Secondly(5);
            widget.TaskExecutorName = "UnitTest-Widget";
            resp = await Client.PostAsJsonAsync<TaskWidget, bool>("/api/Tasks", widget);
            Assert.False(resp);

            widget.TaskExecutorName = "Bootstrap.Admin.DefaultTaskExecutor";
            widget.Name = "UnitTest-Task";
            resp = await Client.PostAsJsonAsync<TaskWidget, bool>("/api/Tasks", widget);
            Assert.True(resp);

            // Delete
            resp = await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("/api/Tasks", new string[] { widget.Name });
            Assert.True(resp);
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
        private HttpClient client;

        public TasksSystemModelTest(BASystemModelWebHost factory)
        {
            client = factory.CreateClient("/api/Tasks");
        }

        [Fact]
        public async void Post_Ok()
        {
            var widget = new TaskWidget();
            widget.CronExpression = Longbow.Tasks.Cron.Secondly(5);
            widget.Name = "单次任务";
            widget.TaskExecutorName = "Bootstrap.Admin.DefaultTaskExecutor";

            // 演示模式下禁止移除系统内置任务
            var resp = await client.PostAsJsonAsync<TaskWidget, bool>("/api/Tasks", widget);
            Assert.False(resp);

            resp = await client.DeleteAsJsonAsync<IEnumerable<string>, bool>("/api/Tasks", new string[] { widget.Name });
            Assert.False(resp);

            widget.Name = "Test-Widget";
            resp = await client.PostAsJsonAsync<TaskWidget, bool>("/api/Tasks", widget);
            Assert.True(resp);

            resp = await client.DeleteAsJsonAsync<IEnumerable<string>, bool>("/api/Tasks", new string[] { widget.Name });
            Assert.True(resp);
        }
    }
}
