using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class TasksTest : Api.TasksTest
    {
        public TasksTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
