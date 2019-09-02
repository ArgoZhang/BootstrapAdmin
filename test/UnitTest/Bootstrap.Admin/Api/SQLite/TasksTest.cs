using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class TasksTest : SqlServer.TasksTest
    {
        public TasksTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
