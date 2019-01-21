using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("BootstrapAdminTestContext")]
    public class TasksTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            var t = new Task();
            Assert.Equal(Enumerable.Empty<Task>(), t.Retrieves());
        }
    }
}
