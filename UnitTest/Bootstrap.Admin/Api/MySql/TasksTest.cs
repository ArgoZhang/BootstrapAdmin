using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class TasksTest : SqlServer.TasksTest
    {
        public TasksTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
