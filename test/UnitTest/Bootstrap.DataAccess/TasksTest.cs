using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class TasksTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            TaskHelper.Save(new Task() { TaskName = "UnitTest", AssignName = "User", UserName = "Admin", TaskTime = 0, TaskProgress = 20, AssignTime = DateTime.Now });
            Assert.NotEmpty(TaskHelper.Retrieves());
        }
    }
}
